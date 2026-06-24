using UnityEngine;
using UnityEngine.UIElements;

public class Steam : MonoBehaviour
{
    [SerializeField] private ParticleSystem system;

    public void Awake()
    {
        SetForce(-3);
    }

    public void SetForce(float force)
    {
        var fol = system.forceOverLifetime;
        fol.x = new ParticleSystem.MinMaxCurve(force, force);
    }

    public void SetSpeed(float speed)
    {
        float force = speed * -0.3f;
        SetForce(force);
    }

    public void SetSteamActive(bool active)
    {
        Debug.Log(active);
        if (active)
        {
            system.Play();
        } else
        {
            system.Stop();
        }
    }
}
