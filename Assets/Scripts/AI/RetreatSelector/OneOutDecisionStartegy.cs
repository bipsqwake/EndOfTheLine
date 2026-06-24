using Unity.VisualScripting;

public class OneOutDecisionStrategy : RetreatDecision
{
    public bool ShouldRetreat(Train train)
    {
        foreach (TrainPart trainPart in train.GetParts())
        {
            if (trainPart.IsDestroyed())
            {
                return true;
            }
        }
        return false;
    }
}