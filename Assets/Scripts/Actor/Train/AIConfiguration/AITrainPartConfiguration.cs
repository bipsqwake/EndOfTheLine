using Newtonsoft.Json;

public abstract class AITrainPartConfiguration
{
    [JsonProperty]
    private float importance;
    [JsonProperty]
    private int maxHealth;

    public float GetImportance()
    {
        return importance;
    }

    public int GetMaxHealth()
    {
        return maxHealth;
    }
    public abstract CarriageType GetCarriageType();
}