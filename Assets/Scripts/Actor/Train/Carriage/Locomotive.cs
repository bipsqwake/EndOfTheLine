using System;
using UnityEngine;

public class Locomotive : Actor
{
    [SerializeField] private Collider2D locomotiveCollider;

    private LocomotiveConfiguration playerConfiguration;
    private AILocomotiveConfiguration enemyConfiguration;
    private TrainPart trainPart;
    private Steam steam;

    public void Awake()
    {
        trainPart = GetComponentInParent<TrainPart>();
        if (trainPart == null)
        {
            throw new ArgumentNullException("Cart should be a child of train part");
        }
        steam = GetComponent<Steam>();
        if (trainPart == null)
        {
            throw new ArgumentNullException("Locomotive should have steam");
        }
    }
    
    public override void SetPlayerControl(bool playerControl)
    {
        base.SetPlayerControl(playerControl);
        locomotiveCollider.gameObject.layer = playerControl ? GlobalSettings.instance.playerLayer : GlobalSettings.instance.enemyLayer;
    }

    public void SetConfiguration(TrainPartConfiguration configuration)
    {
        if (configuration.GetType() != typeof(LocomotiveConfiguration))
        {
            return;
        }
        playerConfiguration = (LocomotiveConfiguration) configuration;
        SetInitHealth(playerConfiguration.GetMaxHealth());
        return;
    }

    public void SetConfiguration(AITrainPartConfiguration configuration)
    {
        if (configuration.GetType() != typeof(AILocomotiveConfiguration))
        {
            return;
        }
        enemyConfiguration = (AILocomotiveConfiguration) configuration;
        SetInitHealth(enemyConfiguration.GetMaxHealth());
        return;
    }

    public float GetImportance()
    {
        return enemyConfiguration.GetImportance();
    }

    public override void ReceiveDamage(int damage)
    {
        base.ReceiveDamage(damage);
        trainPart.DamageCallback();
    }

    public void SetSpeed(float speed)
    {
        steam.SetSpeed(speed);
    }

    public void SetSteamActive(bool active)
    {
        steam.SetSteamActive(active);
    }
}
