using UnityEngine;

public class State : MonoBehaviour
{
    public static State instance;

    [SerializeField] private Train playerTrain;

    public void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
        }
        instance = this;
    }

    public Train GetPlayerTrain()
    {
        return playerTrain;
    }


}
