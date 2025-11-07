using UnityEngine;

public abstract class CarriagePayload : Actor
{
    [SerializeField] private DamageView damageView;
    [SerializeField] private GameObject cartCollider;

    public override void ReceiveDamage(int damage)
    {
        int healthBefore = health;
        base.ReceiveDamage(damage);
        {
            damageView.Activate((float) healthBefore / initHealth, (float) health / initHealth);   
        }
        if (health <= 0)
        {
            DestroyCarriage();
        } 
    }
    
    private void DestroyCarriage()
    {
        cartCollider.SetActive(true);
        Destroy(gameObject);
    }
}
