using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test : MonoBehaviour
{
    public Transform A;
    public Transform B;

    void Update()
    {
        
    }

    public bool isAInFrontOfB(Transform A, Transform B)
    {
        return Vector3.Angle(B.forward, A.position - B.position) <= 90f;
    }

    public Vector2 CalculateQuadraticBezierPoint(Vector2 X, Vector2 Y, Vector2 M, float t)
    {
        // Calculate the quadratic bezier curve position at time t
        float tSquared = t * t;
        float oneMinusT = 1f - t;
        float oneMinusTSquared = oneMinusT * oneMinusT;
        Vector2 position = oneMinusTSquared * X + 2f * t * oneMinusT * M + tSquared * Y;

        return position;
    }

    public Vector3 GetObjectPosition(Vector3 initialPosition, Vector3 V, float t)
    {
        return initialPosition + V * t - 0.5f * V * t * t;
    }
}