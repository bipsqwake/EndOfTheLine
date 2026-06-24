using Newtonsoft.Json;

public class MortairConfiguration : TrainPartConfiguration
{
    [JsonProperty]
    private float reloadTime;
    [JsonProperty]
    private int damage;

    public float GetReloadTime()
    {
        return reloadTime;
    }

    public int GetDamage()
    {
        return damage;
    }

    public MortairConfiguration(float reloadTime, int damage)
    {
        this.reloadTime = reloadTime;
        this.damage = damage;
    }
    public override CarriageType GetCarriageType()
    {
        return CarriageType.MORTAIR;
    }
}