using System;
using UnityEngine;

public class Cart : Actor
{
    [SerializeField] private DamageView damageView;
    [SerializeField] private Collider2D cartCollider;

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
            damageView.Activate((float) healthBefore / initHealth, (float) health / initHealth);   
        }
        if (health <= 0)
        {
            DestroyCart();
        } 
        trainPart.DamageCallback();
    }

    public override void SetPlayerControl(bool playerControl)
    {
        base.SetPlayerControl(playerControl);
        cartCollider.gameObject.layer = playerControl ? GlobalSettings.instance.playerLayer : GlobalSettings.instance.enemyLayer;
    }


    public void SetColliderActive(bool value)
    {
        cartCollider.gameObject.SetActive(value);
    }
    
    private void DestroyCart()
    {
        trainPart.train.ReleaseCarriage(trainPart);
    }
}
