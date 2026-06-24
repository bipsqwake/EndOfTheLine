using UnityEngine;

public class PassangerCarriage : CarriagePayload
{
    private PassangerCartConfiguration playerConfiguration;
    private AIPassangerCartConfiguration enemyConfiguration;
    public override void ReceiveDamage(int damage)
    {
        base.ReceiveDamage(damage);
    }

    public override void SetConfiguration(TrainPartConfiguration configuration)
    {
        if (configuration.GetType() != typeof(PassangerCartConfiguration))
        {
            return;
        }
        playerConfiguration = (PassangerCartConfiguration) configuration;
        SetInitHealth(playerConfiguration.GetMaxHealth());
        return;
    }

    public override void SetConfiguration(AITrainPartConfiguration configuration)
    {
        if (configuration.GetType() != typeof(AIPassangerCartConfiguration))
        {
            return;
        }
        enemyConfiguration = (AIPassangerCartConfiguration) configuration;
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
