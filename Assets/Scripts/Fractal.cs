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

    static float3[] directions = { up(), right(), left(), forward(), back() };
    static quaternion[] rotations = {
        quaternion.identity,
        quaternion.RotateZ(-0.5f * PI),
        quaternion.RotateZ(0.5f * PI),
        quaternion.RotateX(0.5f * PI),
        quaternion.RotateX(-0.5f * PI)
    };
    struct FractalPart {
        public float3 direction;
        public float3 worldPosition;
        public quaternion rotation;
        public quaternion worldRotation;
        public float spinAngle;
    }

    NativeArray<FractalPart>[] parts;
    NativeArray<float4x4>[] matrices;
    ComputeBuffer[] matricesBuffers;

    static readonly int matricesID = Shader.PropertyToID("_Matrices");

    [BurstCompile(FloatPrecision.Standard, FloatMode.Fast, CompileSynchronously = true)]
    struct UpdateFractalLevelJob : IJobFor {
        public float spinAngleDelta;
        public float scale;

        [ReadOnly]
        public NativeArray<FractalPart> parents;
        public NativeArray<FractalPart> parts;

        [WriteOnly]
        public NativeArray<float4x4> matrices;

        public void Execute(int i) {
            FractalPart parent = parents[i / 5];
            FractalPart part = parts[i];

            part.spinAngle += spinAngleDelta;
            part.worldRotation = mul(
                parent.worldRotation,
                mul(part.rotation, quaternion.RotateY(part.spinAngle))
            );
            part.worldPosition = parent.worldPosition +
                mul(parent.worldRotation, 1.5f * scale * part.direction);
            parts[i] = part;

            matrices[i] = float4x4.TRS(part.worldPosition, part.worldRotation, float3(scale));
        }
    }

    void OnEnable() {
        parts = new NativeArray<FractalPart>[depth];
        matrices = new NativeArray<float4x4>[depth];
        matricesBuffers = new ComputeBuffer[depth];

        int stride = 16 * sizeof(float);
        for (int i = 0, length = 1; i < parts.Length; i++, length *= 5) {
            parts[i] = new NativeArray<FractalPart>(length, Allocator.Persistent);
            matrices[i] = new NativeArray<float4x4>(length, Allocator.Persistent);
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
        float spinAngleDelta = 0.125f * PI * Time.deltaTime;

        FractalPart rootPart = parts[0][0];
        rootPart.spinAngle += spinAngleDelta;
        rootPart.worldRotation = mul(
            transform.rotation,
            mul(rootPart.rotation, quaternion.RotateY(rootPart.spinAngle))
        );
        rootPart.worldPosition = transform.position;
        parts[0][0] = rootPart;
        float objectScale = transform.lossyScale.x;
        matrices[0][0] = float4x4.TRS(rootPart.worldPosition, rootPart.worldRotation, float3(objectScale));

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
