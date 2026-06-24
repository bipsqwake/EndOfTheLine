using Newtonsoft.Json;

public class EnemyInfo
{
    [JsonProperty]
    private AITrainConfiguration config;
    [JsonProperty]
    private string name;

    public string GetName()
    {
        return name;
    }
    
    public AITrainConfiguration GetTrainConfiguration()
    {
        return config;
    }
}