public class LocomotiveConfiguration : TrainPartConfiguration
{
    public override CarriageType GetCarriageType()
    {
        return CarriageType.LOCOMOTIVE;
    }
}