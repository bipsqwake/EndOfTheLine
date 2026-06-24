using UnityEngine;

public class Coalcart : CarriagePayload
{
    private CoalCartConfiguration playerConfiguration;
    private AICoalCartConfiguration enemyConfiguration;
    public override void SetConfiguration(TrainPartConfiguration configuration)
    {
        if (configuration.GetType() != typeof(CoalCartConfiguration))
        {
            return;
        }
        playerConfiguration = (CoalCartConfiguration) configuration;
        SetInitHealth(playerConfiguration.GetMaxHealth());
        return;
    }

    public override void SetConfiguration(AITrainPartConfiguration configuration)
    {
        if (configuration.GetType() != typeof(AICoalCartConfiguration))
        {
            return;
        }
        enemyConfiguration = (AICoalCartConfiguration) configuration;
        SetInitHealth(enemyConfiguration.GetMaxHealth());
        return;
    }

    public override AIActionType[] GetActionType()
    {
        return new AIActionType[] {AIActionType.PASSIVE};
    }

    public override float GetImportance()
    {
        return enemyConfiguration.GetImportance();
    }
}
