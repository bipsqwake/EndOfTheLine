using UnityEngine;

public class ArmorCart : CarriagePayload
{
    [SerializeField] private SpriteAim aim;
    [SerializeField] private Shield shield;
    [SerializeField] private ReloadBar reloadBar;

    private ShieldCartConfiguration playerConfiguration;
    private AIShieldCartConfiguration enemyConfiguration;

    private float lastUse;

    public void Start()
    {
        reloadBar.SetReady();
    }
    public void PrepareAttack(Vector2 aimPosition)
    {
        if (!State.instance.IsPlayerControl() || !playerControl || !reloadBar.IsReady())
        {
            return;
        }
        aim.Apply(aimPosition);
    }

    //for player
    public void PerformAttack(Vector2 aimPosition)
    {
        if (!playerControl || !reloadBar.IsReady())
        {
            return;
        }
        if (aimPosition.y < 1.0f)
        {
            aim.Reset();
        }
        else
        {
            reloadBar.Reload(playerConfiguration.GetReloadTime());
            aim.InstantClose();
            shield.Activate(playerConfiguration.GetDuration(), playerConfiguration.GetShieldWidth());
        }
    }

    //for enemy
    public void PerformAttack(float duration, float width)
    {
        shield.Activate(duration, width);
    }

    public override void SetPlayerControl(bool playerControl)
    {
        base.SetPlayerControl(playerControl);

        reloadBar.SetVisible(playerControl);
        shield.SetLayer(playerControl ? GlobalSettings.instance.playerLayer : GlobalSettings.instance.enemyLayer);
    }

    public override void SetConfiguration(TrainPartConfiguration configuration)
    {
        if (configuration.GetType() != typeof(ShieldCartConfiguration))
        {
            return;
        }
        playerConfiguration = (ShieldCartConfiguration) configuration;
        SetInitHealth(playerConfiguration.GetMaxHealth());
        return;
    }

    public override void SetConfiguration(AITrainPartConfiguration configuration)
    {
        if (configuration.GetType() != typeof(AIShieldCartConfiguration))
        {
            return;
        }
        enemyConfiguration = (AIShieldCartConfiguration) configuration;
        SetInitHealth(enemyConfiguration.GetMaxHealth());
        lastUse = 0.0f - enemyConfiguration.GetReloadTime() - 100f; //I know thats kinda lame
        return;
    }

    public override AIActionType[] GetActionType()
    {
        return new AIActionType[] {AIActionType.REACTION};
    }

    public void EnemyActivate()
    {
        shield.Activate(enemyConfiguration.GetDuration(), enemyConfiguration.GetShieldWidth());
        lastUse = Time.time;
    }

    public float GetReloadTimeLeft()
    {
        return Mathf.Max(enemyConfiguration.GetReloadTime() - (Time.time - lastUse), 0.0f);
    }

    public float GetActiveTimeLeft()
    {
        return Mathf.Max(0.0f, enemyConfiguration.GetDuration() - (Time.time - lastUse));
    }

    public float GetShieldWidth()
    {
        return enemyConfiguration.GetShieldWidth();
    }

    public override float GetImportance()
    {
        return enemyConfiguration.GetImportance();
    }
}
