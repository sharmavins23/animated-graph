using UnityEngine;
using Unity.Burst;
using Unity.Collections;
using Unity.Jobs;
using Unity.Mathematics;

using static Unity.Mathematics.math;
using float4x4 = Unity.Mathematics.float4x4;
using quaternion = Unity.Mathematics.quaternion;

public class Fractal : MonoBehaviour {
    [SerializeField, Range(1, 9)] int depth = 4;

    [SerializeField] Mesh mesh;
    [SerializeField] Material material;

    static MaterialPropertyBlock propertyBlock;

    static Vector3[] directions = { up(), right(), left(), forward(), back() };
    static Quaternion[] rotations = {
        Quaternion.identity,
        Quaternion.Euler(0f, 0f, -90f),
        Quaternion.Euler(0f, 0f, 90f),
        Quaternion.Euler(90f, 0f, 0f),
        Quaternion.Euler(-90f, 0f, 0f)
    };
    struct FractalPart {
        public Vector3 direction;
        public Vector3 worldPosition;
        public Quaternion rotation;
        public Quaternion worldRotation;
        public float spinAngle;
    }

    NativeArray<FractalPart>[] parts;
    NativeArray<Matrix4x4>[] matrices;
    ComputeBuffer[] matricesBuffers;

    static readonly int matricesID = Shader.PropertyToID("_Matrices");

    [BurstCompile(CompileSynchronously = true)]
    struct UpdateFractalLevelJob : IJobFor {
        public float spinAngleDelta;
        public float scale;

        [ReadOnly]
        public NativeArray<FractalPart> parents;
        public NativeArray<FractalPart> parts;

        [WriteOnly]
        public NativeArray<Matrix4x4> matrices;

        public void Execute(int i) {
            FractalPart parent = parents[i / 5];
            FractalPart part = parts[i];

            part.spinAngle += spinAngleDelta;
            part.worldRotation = parent.worldRotation * (part.rotation * Quaternion.Euler(0f, part.spinAngle, 0f));
            part.worldPosition = parent.worldPosition + parent.worldRotation * (1.5f * scale * part.direction);
            parts[i] = part;

            matrices[i] = Matrix4x4.TRS(part.worldPosition, part.worldRotation, scale * Vector3.one);
        }
    }

    void OnEnable() {
        parts = new NativeArray<FractalPart>[depth];
        matrices = new NativeArray<Matrix4x4>[depth];
        matricesBuffers = new ComputeBuffer[depth];

        int stride = 16 * sizeof(float);
        for (int i = 0, length = 1; i < parts.Length; i++, length *= 5) {
            parts[i] = new NativeArray<FractalPart>(length, Allocator.Persistent);
            matrices[i] = new NativeArray<Matrix4x4>(length, Allocator.Persistent);
            matricesBuffers[i] = new ComputeBuffer(length, stride);
        }

        parts[0][0] = CreatePart(0);
        for (int li = 1; li < parts.Length; li++) {
            NativeArray<FractalPart> levelParts = parts[li];

            for (int fpi = 0; fpi < levelParts.Length; fpi += 5) {
                for (int ci = 0; ci < 5; ci++) levelParts[fpi + ci] = CreatePart(ci);
            }
        }

        propertyBlock ??= new MaterialPropertyBlock();
    }

    void OnDisable() {
        for (int i = 0; i < matricesBuffers.Length; i++) {
            matricesBuffers[i].Release();
            parts[i].Dispose();
            matrices[i].Dispose();
        }
        matricesBuffers = null;
        matrices = null;
        parts = null;
    }

    void OnValidate() {
        if (parts != null && enabled) {
            OnDisable();
            OnEnable();
        }
    }

    void Update() {
        float spinAngleDelta = 22.5f * Time.deltaTime;

        FractalPart rootPart = parts[0][0];
        rootPart.spinAngle += spinAngleDelta;
        rootPart.worldRotation = transform.rotation * (rootPart.rotation * Quaternion.Euler(0f, rootPart.spinAngle, 0f));
        rootPart.worldPosition = transform.position;
        parts[0][0] = rootPart;
        float objectScale = transform.lossyScale.x;
        matrices[0][0] = Matrix4x4.TRS(rootPart.worldPosition, rootPart.worldRotation, objectScale * Vector3.one);

        float scale = objectScale;
        JobHandle jobHandle = default;
        for (int li = 1; li < parts.Length; li++) {
            scale *= 0.5f;
            jobHandle = new UpdateFractalLevelJob {
                spinAngleDelta = spinAngleDelta,
                scale = scale,
                parents = parts[li - 1],
                parts = parts[li],
                matrices = matrices[li]
            }.Schedule(parts[li].Length, jobHandle);
        }
        jobHandle.Complete();

        // Upload matrices to the GPU
        Bounds bounds = new(rootPart.worldPosition, 3f * objectScale * Vector3.one);
        for (int i = 0; i < matricesBuffers.Length; i++) {
            ComputeBuffer buffer = matricesBuffers[i];
            buffer.SetData(matrices[i]);
            propertyBlock.SetBuffer(matricesID, buffer);
            Graphics.DrawMeshInstancedProcedural(mesh, 0, material, bounds, buffer.count, propertyBlock);
        }
    }

    FractalPart CreatePart(int childIndex) => new FractalPart {
        direction = directions[childIndex],
        rotation = rotations[childIndex]
    };
}
