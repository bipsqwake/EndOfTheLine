using Unity.VisualScripting;
using System.Collections;
using UnityEngine;
using System;

public abstract class CarriagePayload : Actor
{
    [SerializeField] private DamageView damageView;
    [SerializeField] private Collider2D carriageCollider;
    [SerializeField] private Cart cart;
    private TrainPart trainPart;

    public void Awake()
    {
        trainPart = GetComponentInParent<TrainPart>();
        if (trainPart == null)
        {
            throw new ArgumentNullException("Cart should be a child of train part");
        }
    }


    public override void ReceiveDamage(int damage)
    {
        int healthBefore = health;
        base.ReceiveDamage(damage);
        if (damageView != null)
        {
            damageView.Activate((float)healthBefore / initHealth, (float)health / initHealth);
        }
        if (health <= 0)
        {
            DestroyCarriage();
        }
        trainPart.DamageCallback();
    }

    public override void SetPlayerControl(bool playerControl)
    {
        base.SetPlayerControl(playerControl);
        carriageCollider.gameObject.layer = playerControl ? GlobalSettings.instance.playerLayer : GlobalSettings.instance.enemyLayer;
    }

    public abstract void SetConfiguration(TrainPartConfiguration configuration);
    public abstract void SetConfiguration(AITrainPartConfiguration configuration);
    

    private void DestroyCarriage()
    {
        cart.SetColliderActive(true);
        gameObject.SetActive(false);
        foreach (Transform child in transform)
        {
            child.gameObject.SetActive(false);
        }
        damageView.gameObject.SetActive(false);
        destroyed = true; 
    }

    public virtual IEnumerator CoroutineAction()
    {
        yield return null;
    }

    public abstract AIActionType[] GetActionType();

    public abstract float GetImportance();
}
