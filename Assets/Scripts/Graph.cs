using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class Graph : MonoBehaviour {
    [SerializeField] Transform pointPrefab;
    [SerializeField, Range(10, 100)] int resolution = 10;

    // Start is called before the first frame update
    void Start() {
        float step = 2f / resolution;
        Vector3 scale = Vector3.one * step;
        Vector3 position = new Vector3();

        for (int i = 0; i < resolution; i++) {
            Transform point = Instantiate(pointPrefab); // Create point
            point.SetParent(transform, false); // Set parent to Graph object

            // Graph function
            position.x = (i + 0.5f) * step - 1f;
            position.y = (float)Math.Pow(position.x, 3);

            point.localPosition = position;
            point.localScale = scale;
        }
    }

    // Update is called once per frame
    void Update() {

    }
}
