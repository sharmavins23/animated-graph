using System;
using System.IO.Compression;
using UnityEngine;

public class Graph : MonoBehaviour {
    [SerializeField] Transform pointPrefab;
    [SerializeField, Range(10, 100)] int resolution = 10;
    [SerializeField] FunctionLibrary.FunctionName function;

    Transform[] points;

    // Start is called before the first frame update
    void Start() {
        // Create a depth-based grid by creating a series of points
        points = new Transform[resolution * resolution];

        float step = 2f / resolution;
        Vector3 scale = Vector3.one * step;
        Vector3 position = new Vector3();

        for (int i = 0, x = 0, z = 0; i < points.Length; i++, x++) {
            // Reset which line we're on
            if (x == resolution) {
                x = 0;
                z += 1;
            }

            Transform point = points[i] = Instantiate(pointPrefab); // Create point
            point.SetParent(transform, false); // Set parent to Graph object
            position.x = (x + 0.5f) * step - 1f;
            position.z = (z + 0.5f) * step - 1f;
            point.localPosition = position;
            point.localScale = scale;
        }
    }

    // Update is called once per frame
    void Update() {
        float time = Time.time;
        FunctionLibrary.Function function = FunctionLibrary.GetFunction(this.function);

        for (int i = 0; i < points.Length; i++) {
            Transform point = points[i];
            Vector3 position = point.localPosition;

            // Graph the function
            position.y = function(position.x, position.z, time);

            point.localPosition = position;
        }
    }
}
