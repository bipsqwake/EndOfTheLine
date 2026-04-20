using System.Collections.Generic;
using UnityEngine;

public class DebugGizmos : MonoBehaviour
{

    private static List<Vector3> lines = new();
    private static List<Vector3> dots = new();
    private static bool draw = false;

    public static void AddLine(Vector3 coord)
    {
        draw = true;
        DebugGizmos.lines.Add(coord);
    }

    public static void AddDot(Vector3 coord)
    {
        dots.Add(coord);
    }

    public static void Clear()
    {
        lines.Clear();
        dots.Clear();
    }

    public static void Close()
    {
        draw = false;
    }


    public void OnDrawGizmos()
    {
        foreach (var c in lines)
        {
            // Gizmos.DrawSphere(c, 0.1f);
            Gizmos.DrawLine(c + Vector3.up, c + Vector3.down);
        }
        foreach (var c in dots)
        {
            // Gizmos.DrawSphere(c, 0.1f);
            Gizmos.DrawSphere(c, 0.1f);
        }
    }
}
