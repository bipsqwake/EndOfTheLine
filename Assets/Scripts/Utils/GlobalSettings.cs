using UnityEngine;

public class GlobalSettings : MonoBehaviour
{
    public static GlobalSettings instance;

    public int enemyLayer;
    public int playerLayer;
    public int enemyProjectileLayer;
    public int playerProjectileLayer;
    public void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
        }
        instance = this;
    }
}
