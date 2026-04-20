using UnityEngine;

[CreateAssetMenu(fileName = "AICoalCart", menuName = "AICarriage/AICoalCart")]
public class AICoalCart : AICarriage
{
    public override AIActionType[] GetActionType()
    {
        return new AIActionType[] { AIActionType.PASSIVE };
    }
}
