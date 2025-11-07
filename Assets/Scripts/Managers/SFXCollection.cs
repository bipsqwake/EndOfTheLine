using UnityEngine;

public class SFXCollection : MonoBehaviour
{
    public static SFXCollection instance;
    [SerializeField] private AudioClip explotion;

    public void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
        }
        instance = this;
    }

    public void Explotion()
    {
        SoundManager.instance.Play(explotion);
    }
}
