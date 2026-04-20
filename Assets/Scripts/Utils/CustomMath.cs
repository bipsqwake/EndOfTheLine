using UnityEngine;

public class CustomMath
{
    public static float ExpDec(float m, float p, float x) 
    {
        return m + (1 - m) * Mathf.Pow(x, p);
    }
}
