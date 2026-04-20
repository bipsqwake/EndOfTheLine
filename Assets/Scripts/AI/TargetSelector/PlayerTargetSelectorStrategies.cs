public class PlayerTargetSelectorStrategies
{
    public enum Strategy
    {
        DEFAULT
    }

    public static PlayerTargetSelector GetSelector(Strategy strategy)
    {
        return strategy switch
        {
            _ => new DefaultPlayerTargetSelector(),
        };
    }
}