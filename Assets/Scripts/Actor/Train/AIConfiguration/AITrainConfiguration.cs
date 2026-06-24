using System.Collections.Generic;
using Newtonsoft.Json;

public class AITrainConfiguration
{
    [JsonProperty]
    private List<AITrainPartConfiguration> config = new();
    [JsonProperty]
    private float cartImportance;
    [JsonProperty]
    private float impactWeigth;
    [JsonProperty]
    private RetreatDecisionStrategies.Strategy retreatStrategy;

    public List<AITrainPartConfiguration> GetConfig()
    {
        return config;
    }

    public float GetCartImportance()
    {
        return cartImportance;
    }

    public float GetImpactWeigth()
    {
        return impactWeigth;
    }

    public RetreatDecisionStrategies.Strategy GetRetreatStrategy()
    {
        return retreatStrategy;
    }

    public void Add(AITrainPartConfiguration c)
    {
        config.Add(c);
    }
}