using Newtonsoft.Json;
using NUnit.Framework.Constraints;
using UnityEngine;

public class GameLoader : MonoBehaviour
{
    [SerializeField] private Transform gameRoot;
    [SerializeField] private Train trainPrefab;
    [SerializeField] private Transform newGamePosition;
    [SerializeField] private NewGameDecorations newGameDecorations;
    [SerializeField] private TextAsset trainConfig;
    void Start()
    {
        
        if (MainSceneLoadInfo.newGame)
        {
            NewGame();
        } else
        {
            NewGame();
        }
    }

    private TrainConfiguration GetNewGameConfig()
    {
        // TrainConfiguration result = new();
        // LocomotiveConfiguration locomotiveConfiguration = new();
        // PassangerCartConfiguration passangerCartConfiguration = new();
        // MortairConfiguration mortairConfiguration = new(5.0f, 10);
        // result.AddConfig(locomotiveConfiguration);
        // result.AddConfig(passangerCartConfiguration);
        // result.AddConfig(passangerCartConfiguration);
        // result.AddConfig(mortairConfiguration);
        // Debug.Log(JsonConvert.SerializeObject(result, new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.All }));

        TrainConfiguration result = JsonConvert.DeserializeObject<TrainConfiguration>(trainConfig.text, new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.All });

        return result;
    }

    private void NewGame()
    {
        Train train = Instantiate(trainPrefab, newGamePosition.position, Quaternion.identity, gameRoot);
        train.InitializeFromConfiguration(GetNewGameConfig());
        State.instance.SetPlayerTrain(train);
        newGameDecorations.SetTrain(train);
        newGameDecorations.StartNewGameSequence();
    }
}
