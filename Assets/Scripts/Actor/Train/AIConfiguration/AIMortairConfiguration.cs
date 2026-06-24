using Newtonsoft.Json;

public class AIMortairConfiguration : AITrainPartConfiguration
{
    [JsonProperty]
    private float minReloadTime;
    [JsonProperty]
    private float maxReloadTime;
    [JsonProperty]
    private float minStartDelay;
    [JsonProperty]
    private float maxStartDealy;
    [JsonProperty]
    private PlayerTargetSelectorStrategies.Strategy strategy;

    public float GetMinReloadTime()
    {
        return minReloadTime;
    }

    public float GetMaxReloadTime()
    {
        return maxReloadTime;
    }

    public float GetMinStartDelay()
    {
        return minStartDelay;
    }

    public float GetMaxStartDelay()
    {
        return maxStartDealy;
    }

    public PlayerTargetSelectorStrategies.Strategy GetStrategy()
    {
        return strategy;
    }

    public override CarriageType GetCarriageType()
    {
        return CarriageType.MORTAIR;
    }
}