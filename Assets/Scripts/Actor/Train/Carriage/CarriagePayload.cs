using Unity.VisualScripting;
using UnityEngine;

public abstract class CarriagePayload : Actor
{
    [SerializeField] private DamageView damageView;
    [SerializeField] private Collider2D carriageCollider;
    [SerializeField] private Cart cart;


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
    }

    public override void SetPlayerControl(bool playerControl)
    {
        base.SetPlayerControl(playerControl);
        carriageCollider.gameObject.layer = playerControl ? GlobalSettings.instance.playerLayer : GlobalSettings.instance.enemyLayer;
    }
    

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
}
