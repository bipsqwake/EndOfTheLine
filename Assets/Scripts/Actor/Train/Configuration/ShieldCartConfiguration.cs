using Newtonsoft.Json;

public class ShieldCartConfiguration : TrainPartConfiguration
{
    [JsonProperty] 
    private float duration;
    [JsonProperty] 
    private float reloadTime;
    [JsonProperty]
    private float shieldWidth;

    public float GetDuration()
    {
        return duration;
    }

    public float GetReloadTime()
    {
        return reloadTime;
    }

    public float GetShieldWidth()
    {
        return shieldWidth;
    }
    public override CarriageType GetCarriageType()
    {
        return CarriageType.SHIELD;
    }
}