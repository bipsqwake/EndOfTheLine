using System;
using UnityEngine;

public class Cart : Actor
{
    [SerializeField] private DamageView damageView;

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
    }
    
    private void DestroyCart()
    {
        TrainPart trainPart = GetComponentInParent<TrainPart>();
        if (trainPart == null)
        {
            throw new ArgumentNullException("Cart should be a child of train part");
        }
        trainPart.train.ReleaseCarriage(trainPart);
    }
}
