using UnityEngine;

public class Actor : MonoBehaviour
{
    [SerializeField] protected int initHealth;

    protected int health;

    protected bool playerControl;
    protected bool destroyed = false;

    // public void Awake()
    // {
    //     health = initHealth;
    // }

    public void SetInitHealth(int health)
    {
        this.initHealth = health;
        this.health = health;
    }

    public virtual void ReceiveDamage(int damage)
    {
        health -= damage;
        if (health >= 0)
        {
            destroyed = true;
        }
    }

    public virtual void SetPlayerControl(bool playerControl)
    {
        this.playerControl = playerControl;
    }

    public bool IsDestroyed()
    {
        return destroyed;
    }

    public int GetHealth()
    {
        return health;
    }

    public int GetDamage()
    {
        return initHealth - health;
    }

    public float GetHealthPercent()
    {
        return (float)health / initHealth;
    }
 }
