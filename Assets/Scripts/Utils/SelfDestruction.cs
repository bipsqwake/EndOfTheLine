using UnityEngine;

public class SelfDestruction : MonoBehaviour
{
    [SerializeField] private float seconds;
    public void Start()
    {
        Destroy(gameObject, seconds);
    }
}
