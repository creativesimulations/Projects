using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ActualAngle
{

    public static float Repeat(float t, float length)
    {
        return t - Mathf.Floor(t / length) * length;
    }

}