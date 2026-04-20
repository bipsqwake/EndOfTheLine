using UnityEngine;

[CreateAssetMenu(fileName = "AILocomotive", menuName = "AICarriage/AILocomotive")]
public class AILocomotive : AICarriage
{
    public override AIActionType[] GetActionType()
    {
        return new AIActionType[] { AIActionType.PASSIVE };
    }
}
