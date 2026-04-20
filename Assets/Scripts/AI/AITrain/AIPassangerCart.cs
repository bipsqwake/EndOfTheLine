using UnityEngine;

[CreateAssetMenu(fileName = "AIPassangerCart", menuName = "AICarriage/AIPassangerCart")]
public class AIPassangerCart : AICarriage
{
    public override AIActionType[] GetActionType()
    {
        return new AIActionType[] { AIActionType.PASSIVE };
    }
}
