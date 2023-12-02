using System;
using UnityEngine;
using UnityEngine.Assertions;

public class GPUGraph : MonoBehaviour {
    public enum TransitionMode { Pick, Cycle, Random };

    [SerializeField] ComputeShader computeShader;
    [SerializeField] Material material;
    [SerializeField] Mesh mesh;

    const int maxResolution = 1000;
    [SerializeField, Range(10, maxResolution)] int resolution = 100;
    [SerializeField] FunctionLibrary.FunctionName function;
    [SerializeField] TransitionMode transitionMode;
    [SerializeField, Min(0f)] float functionDuration = 1f, transitionDuration = 1f;

    static readonly int positionsID = Shader.PropertyToID("_Positions");
    static readonly int resolutionID = Shader.PropertyToID("_Resolution");
    static readonly int stepID = Shader.PropertyToID("_Step");
    static readonly int timeID = Shader.PropertyToID("_Time");
    static readonly int transitionProgressID = Shader.PropertyToID("_TransitionProgress");

    float duration;
    bool transitioning;
    FunctionLibrary.FunctionName transitionFunction;

    ComputeBuffer positionsBuffer;

    // Invoked each time the object is enabled
    void OnEnable() {
        positionsBuffer = new ComputeBuffer(maxResolution * maxResolution, sizeof(float) * 3);
        Assert.IsTrue(sizeof(float) == 4);
    }

    void OnDisable() {
        positionsBuffer.Release();
        positionsBuffer = null;
    }

    // Update is called once per frame
    void Update() {
        duration += Time.deltaTime;
        if (transitioning) {
            if (duration >= transitionDuration) {
                duration -= transitionDuration;
                transitioning = false;
            }
        } else if (duration >= functionDuration) {
            duration -= functionDuration;

            transitioning = true;
            transitionFunction = function;

            PickNextFunction();
        }

        SetFunctionOnGPU();
    }

    void SetFunctionOnGPU() {
        float step = 2f / resolution;
        computeShader.SetInt(resolutionID, resolution);
        computeShader.SetFloat(stepID, step);
        computeShader.SetFloat(timeID, Time.time);

        if (transitioning) {
            computeShader.SetFloat(transitionProgressID, Mathf.SmoothStep(0f, 1f, duration / transitionDuration));
        }

        int kernelIndex = (int)function + (int)(transitioning ? transitionFunction : function) * FunctionLibrary.FunctionCount;
        computeShader.SetBuffer(kernelIndex, positionsID, positionsBuffer);

        int groups = Mathf.CeilToInt(resolution / 8f);
        computeShader.Dispatch(kernelIndex, groups, groups, 1);

        material.SetBuffer(positionsID, positionsBuffer);
        material.SetFloat(stepID, step);
        Bounds bounds = new(Vector3.zero, Vector3.one * (2f + 2f / resolution));
        Graphics.DrawMeshInstancedProcedural(mesh, 0, material, bounds, resolution * resolution);
    }

    void PickNextFunction() {
        if (transitionMode == TransitionMode.Pick) {
            // Don't change the function
        } else if (transitionMode == TransitionMode.Cycle) {
            function = FunctionLibrary.GetNextFunctionName(function);
        } else {
            function = FunctionLibrary.GetRandomFunctionNameOtherThan(function);
        }
    }
}
