using UnityEngine;

public class State : MonoBehaviour
{
    public static State instance;

    private Train playerTrain;

    private bool playerControl = false;

    public void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
        }
        instance = this;
    }

    public void SetPlayerTrain(Train train)
    {
        this.playerTrain = train;
    }

    public Train GetPlayerTrain()
    {
        return playerTrain;
    }

    public bool IsPlayerControl()
    {
        return playerControl;
    }

    public void SetPlayerControl(bool playerControl)
    {
        this.playerControl = playerControl;
    }


}
