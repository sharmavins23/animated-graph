using UnityEngine;
using UnityEngine.Assertions;
using static UnityEngine.Mathf;

public static class FunctionLibrary {
    public delegate Vector3 Function(float u, float v, float t);
    static Function[] functions = {
        Wave,
        MultiWave,
        Ripple,
        Spider,
        Sphere,
        BouncyBall,
        VerticalBandedSphere,
        HorizontalBandedSphere,
        TwistingSphere,
        SpindleTorus,
        RingTorus,
        SpiralStar,
        LineHelix,
        SinRS,
        CosRS,
        TanRS,
        SecRS,
        TanStar,
        Enneper,
        DuplinCyclide
    };
    public enum FunctionName {
        Wave,
        MultiWave,
        Ripple,
        Spider,
        Sphere,
        BouncyBall,
        VerticalBandedSphere,
        HorizontalBandedSphere,
        TwistingSphere,
        SpindleTorus,
        RingTorus,
        SpiralStar,
        LineHelix,
        SinRS,
        CosRS,
        TanRS,
        SecRS,
        TanStar,
        Enneper,
        DuplinCyclide
    };

    public static Function GetFunction(FunctionName name) {
        return functions[(int)name];
    }

    public static Vector3 Wave(float u, float v, float t) {
        Vector3 p;

        p.x = u;
        p.y = Sin(PI * (u + v + t));
        p.z = v;

        return p;
    }

    public static Vector3 MultiWave(float u, float v, float t) {
        Vector3 p;

        p.x = u;
        float y = Sin(PI * (u + 0.5f + t));
        y += 0.5f * Sin(2f * PI * (v + t));
        y += Sin(PI * (u + v + 0.25f * t));
        p.y = y * (2f / 3f);
        p.z = v;

        return p;
    }

    public static Vector3 Ripple(float u, float v, float t) {
        Vector3 p;

        p.x = u;
        float d = Sqrt(u * u + v * v);
        float y = Sin(PI * (4f * d - t));
        p.y = y / (1f + 10f * d);
        p.z = v;

        return p;
    }

    public static Vector3 Spider(float u, float v, float t) {
        Vector3 p;

        p.x = u;
        p.y = u * v * Sin(PI * u * v * t);
        p.z = v;

        return p;
    }

    public static Vector3 Sphere(float u, float v, float t) {
        Vector3 p;

        float r = Cos(0.5f * PI * v);
        p.x = r * Sin(PI * u);
        p.y = Sin(PI * 0.5f * v);
        p.z = r * Cos(PI * u);

        return p;
    }

    public static Vector3 BouncyBall(float u, float v, float t) {
        Vector3 p;

        float r = 0.5f + 0.5f * Sin(PI * t);
        float s = r * Cos(0.5f * PI * v);
        p.x = s * Sin(PI * u);
        p.y = r * Sin(PI * 0.5f * v);
        p.z = s * Cos(PI * u);

        return p;
    }

    public static Vector3 VerticalBandedSphere(float u, float v, float t) {
        Vector3 p;

        float r = 0.9f + 0.1f * Sin(8f * PI * u);
        float s = r * Cos(0.5f * PI * v);
        p.x = s * Sin(PI * u);
        p.y = r * Sin(PI * 0.5f * v);
        p.z = s * Cos(PI * u);

        return p;
    }

    public static Vector3 HorizontalBandedSphere(float u, float v, float t) {
        Vector3 p;

        float r = 0.9f + 0.1f * Sin(8f * PI * v);
        float s = r * Cos(0.5f * PI * v);
        p.x = s * Sin(PI * u);
        p.y = r * Sin(PI * 0.5f * v);
        p.z = s * Cos(PI * u);

        return p;
    }

    public static Vector3 TwistingSphere(float u, float v, float t) {
        Vector3 p;

        float r = 0.9f + 0.1f * Sin(PI * (6f * u + 4f * v + t));
        float s = r * Cos(0.5f * PI * v);
        p.x = s * Sin(PI * u);
        p.y = r * Sin(PI * 0.5f * v);
        p.z = s * Cos(PI * u);

        return p;
    }

    public static Vector3 SpindleTorus(float u, float v, float t) {
        Vector3 p;

        float r = 1f;
        float s = 0.5f + r * Cos(PI * v);
        p.x = s * Sin(PI * u);
        p.y = r * Sin(PI * v);
        p.z = s * Cos(PI * u);

        return p;
    }

    public static Vector3 RingTorus(float u, float v, float t) {
        Vector3 p;

        float r1 = 0.75f;
        float r2 = 0.25f;
        float s = r1 + r2 * Cos(PI * v);
        p.x = s * Sin(PI * u);
        p.y = r2 * Sin(PI * v);
        p.z = s * Cos(PI * u);

        return p;
    }

    public static Vector3 SpiralStar(float u, float v, float t) {
        Vector3 p;

        float r1 = 0.7f + 0.1f * Sin(PI * (6f * u + 0.5f * t));
        float r2 = 0.15f + 0.05f * Sin(PI * (8f * u + 4f * v + 2f * t));
        float s = r1 + r2 * Cos(PI * v);
        p.x = s * Sin(PI * u);
        p.y = r2 * Sin(PI * v);
        p.z = s * Cos(PI * u);

        return p;
    }

    public static Vector3 LineHelix(float u, float v, float t) {
        Vector3 p;

        p.x = u * Cos(v * t);
        p.y = u * Sin(v * t);
        p.z = v;

        return p;
    }

    public static Vector3 SinRS(float u, float v, float t) {
        Vector3 p;

        p.x = u * ((1 - Pow(v, 2)) / (1 + Pow(v, 2)));
        p.y = u * (2 * v / (1 + Pow(v, 2)));
        p.z = Sin(5f * u * t);

        return p;
    }

    public static Vector3 CosRS(float u, float v, float t) {
        Vector3 p;

        p.x = u * ((1 - Pow(v, 2)) / (1 + Pow(v, 2)));
        p.y = u * (2 * v / (1 + Pow(v, 2)));
        p.z = Cos(5f * u * t);

        return p;
    }

    public static Vector3 TanRS(float u, float v, float t) {
        Vector3 p;

        p.x = u * ((1 - Pow(v, 2)) / (1 + Pow(v, 2)));
        p.y = u * (2 * v / (1 + Pow(v, 2)));
        p.z = Tan(u * t);

        return p;
    }

    public static Vector3 SecRS(float u, float v, float t) {
        Vector3 p;

        p.x = u * ((1 - Pow(v, 2)) / (1 + Pow(v, 2)));
        p.y = u * (2 * v / (1 + Pow(v, 2)));
        p.z = 1f / Cos(u * t);

        return p;
    }

    public static Vector3 TanStar(float u, float v, float t) {
        Vector3 p;

        float r1 = 0.7f + 0.1f * Tan(PI * (u + 0.5f * t));
        float r2 = 0.15f + 0.05f * Tan(PI * (u + 4f * v + 2f * t));
        float s = r1 + r2 * Cos(PI * v);
        p.x = s * Sin(PI * u);
        p.y = r2 * Sin(PI * v);
        p.z = s * Cos(PI * u);

        return p;
    }

    public static Vector3 Enneper(float u, float v, float t) {
        Vector3 p;

        float oneThirds = 1f / 3f;
        p.x = oneThirds * u * (1 - oneThirds * Pow(u, 2) + Pow(v, 2));
        p.y = oneThirds * v * (1 - oneThirds * Pow(v, 2) + Pow(u, 2));
        p.z = oneThirds * (Pow(u, 2) - Pow(v, 2));

        // Add rotation to the object over time
        p = Quaternion.Euler(0f, 30f * t, 0f) * p;

        return p;
    }

    public static Vector3 DuplinCyclide(float u, float v, float t) {
        Vector3 p;

        float a = 1f;
        float b = 1f;
        float c = 1f;
        float d = 1f;

        p.x = (d * (c - a * Cos(u) * Cos(v)) + Pow(b, 2) * Cos(u)) / (a - c * Cos(u) * Cos(v));
        p.y = b * Sin(u) * (a - d * Cos(v)) / (a - c * Cos(u) * Cos(v));
        p.z = b * Sin(v) * (c * Cos(u) - d) / (a - c * Cos(u) * Cos(v));

        // Add rotation to the object over time
        p = Quaternion.Euler(0f, 30f * t, 90f) * p;

        // Shift the object down
        p.y -= 3f;

        return p;
    }
}
