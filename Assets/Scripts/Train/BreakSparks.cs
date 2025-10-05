using System.Collections.Generic;
using UnityEngine;

public class BreakSparks : MonoBehaviour
{
    [SerializeField] private List<ParticleSystem> sparks;

    public void Activate(bool activate)
    {
        foreach (var sparkEmmiter in sparks)
        {
            if (activate)
            {
                sparkEmmiter.Play();
            }
            else
            {
                sparkEmmiter.Stop();
            }
        }
    }
}
