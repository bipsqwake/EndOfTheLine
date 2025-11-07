using UnityEngine;

public class Actor : MonoBehaviour
{
    [SerializeField] protected int initHealth;

    protected int health;

    public void Awake()
    {
        health = initHealth;
    }

    public virtual void ReceiveDamage(int damage)
    {
        health -= damage;
    }
 }
