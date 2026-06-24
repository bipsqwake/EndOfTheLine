public class RetreatDecisionStrategies
{
    public enum Strategy
    {
        ONE_OUT   
    }

    public static RetreatDecision GetDecision(Strategy strategy)
    {
        return strategy switch
        {
            Strategy.ONE_OUT => new OneOutDecisionStrategy(),
            _ => new OneOutDecisionStrategy()
        };
    }
}