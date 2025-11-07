using System;
using UnityEngine;

public class DamageReceiver : MonoBehaviour
{
    [SerializeField] private Actor actor;

    public void Awake()
    {
        
    }

    public void ReceiveDamage(int damage)
    {
        if (actor != null)
        {
            actor.ReceiveDamage(damage);    
        }
    }
}
