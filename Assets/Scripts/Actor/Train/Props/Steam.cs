using UnityEngine;
using UnityEngine.UIElements;

public class Steam : MonoBehaviour
{
    [SerializeField] private ParticleSystem system;
    private bool force = true;

    public void Awake()
    {
        SetForce(-3);
    }

    public void SetForce(float force)
    {
        var fol = system.forceOverLifetime;
        fol.x = new ParticleSystem.MinMaxCurve(force, force);
    }
}
