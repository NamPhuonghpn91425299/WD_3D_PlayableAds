using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class MathHr
{
    public static float Remap(float main, float minIn, float maxIn, float minOut, float maxOut)
    {
        if (maxIn - minIn == 0)
            return (maxOut + minOut) / 2;
        return minOut + (main - minIn) * (maxOut - minOut) / (maxIn - minIn);
    }
}
