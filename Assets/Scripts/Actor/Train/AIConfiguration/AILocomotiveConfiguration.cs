public class AILocomotiveConfiguration : AITrainPartConfiguration
{
    public override CarriageType GetCarriageType()
    {
        return CarriageType.LOCOMOTIVE;
    }
}