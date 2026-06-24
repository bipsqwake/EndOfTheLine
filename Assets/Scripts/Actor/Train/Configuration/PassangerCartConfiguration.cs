public class PassangerCartConfiguration : TrainPartConfiguration
{
    public override CarriageType GetCarriageType()
    {
        return CarriageType.PASSANGER;
    }
}