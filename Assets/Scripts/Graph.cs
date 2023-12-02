using System;
using System.IO.Compression;
using UnityEngine;

public class Graph : MonoBehaviour {
    [SerializeField] Transform pointPrefab;
    [SerializeField, Range(10, 100)] int resolution = 100;
    [SerializeField] FunctionLibrary.FunctionName function;
    [SerializeField, Min(0f)] float functionDuration = 1f, transitionDuration = 1f;
    public enum TransitionMode { Pick, Cycle, Random };
    [SerializeField] TransitionMode transitionMode;

    Transform[] points;
    float duration;
    bool transitioning;
    FunctionLibrary.FunctionName transitionFunction;

    // Start is called before the first frame update
    void Start() {
        // Create a depth-based grid by creating a series of points
        points = new Transform[resolution * resolution];

        float step = 2f / resolution;
        Vector3 scale = Vector3.one * step;

        for (int i = 0; i < points.Length; i++) {
            Transform point = points[i] = Instantiate(pointPrefab);
            point.localScale = scale;
            point.SetParent(transform, false);
        }
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

        if (transitioning) SetTransition();
        else SetFunction();
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

    void SetTransition() {
        FunctionLibrary.Function from = FunctionLibrary.GetFunction(transitionFunction);
        FunctionLibrary.Function to = FunctionLibrary.GetFunction(function);

        float progress = duration / transitionDuration;
        float time = Time.time;
        float step = 2f / resolution;

        float v = 0.5f * step - 1f;
        for (int i = 0, x = 0, z = 0; i < points.Length; i++, x++) {
            // Reset our line
            if (x == resolution) {
                x = 0;
                z += 1;
                v = (z + 0.5f) * step - 1f;
            }
            float u = (x + 0.5f) * step - 1f;
            points[i].localPosition = FunctionLibrary.Morph(u, v, time, from, to, progress);
        }
    }

    void SetFunction() {
        FunctionLibrary.Function function = FunctionLibrary.GetFunction(this.function);

        float time = Time.time;
        float step = 2f / resolution;

        float v = 0.5f * step - 1f;
        for (int i = 0, x = 0, z = 0; i < points.Length; i++, x++) {
            // Reset our line
            if (x == resolution) {
                x = 0;
                z += 1;
                v = (z + 0.5f) * step - 1f;
            }
            float u = (x + 0.5f) * step - 1f;
            points[i].localPosition = function(u, v, time);
        }
    }
}
