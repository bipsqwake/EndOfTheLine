using Newtonsoft.Json;

public abstract class TrainPartConfiguration
{
    [JsonProperty]
    private int maxHealth;

    public int GetMaxHealth()
    {
        return maxHealth;
    }
    public abstract CarriageType GetCarriageType();
}