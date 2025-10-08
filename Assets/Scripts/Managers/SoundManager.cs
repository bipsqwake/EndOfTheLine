using UnityEngine;

public class SoundManager : MonoBehaviour
{

    public static SoundManager instance;
    [SerializeField] private AudioSource audioSource;

    public void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
        }
        instance = this;
    }

    public void Play(AudioClip clip)
    {
        audioSource.PlayOneShot(clip);
    }
}
