using System;
using UnityEditor.ShaderGraph.Internal;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    [SerializeField] private float v0;
    [SerializeField] private float g;
    [SerializeField] private GameObject explotion;
    [SerializeField] private AudioClip sound;
    [SerializeField] private int damage;

    private Vector3 initPosition;
    private float angle;
    private int xDirection;
    private float time;
    private float groundY;
    private int yDirection;
    private float prevY;
    private int targetLayer;
    public void Init(float angle, float groundY, int targetLayer)
    {
        this.initPosition = transform.position;
        this.angle = Mathf.Deg2Rad * (90.0f - Mathf.Abs(angle));
        this.xDirection = (int)Mathf.Sign(angle);
        this.yDirection = 1;
        this.prevY = transform.position.y;
        this.groundY = groundY;
        this.targetLayer = targetLayer;
        time = 0.0f;
    }

    public void Init(Vector2 localTarget, float groundY, int targetLayer)
    {
        this.initPosition = transform.position;
        this.groundY = groundY;
        this.angle = CalculateAngle(localTarget);
        this.xDirection = (int)Mathf.Sign(localTarget.x);
        this.yDirection = 1;
        this.prevY = transform.position.y;
        this.targetLayer = targetLayer;
        time = 0.0f;
    }

    public void Update()
    {
        time += Time.deltaTime;
        float y = CalculateY(time);
        if (y < prevY)
        {
            yDirection = -1;
        }
        prevY = y;
        transform.position = initPosition + new Vector3(xDirection * CalculateX(time), y);
        DestroyCheck();
    }

    private float CalculateY(float t)
    {
        return v0 * Mathf.Sin(angle) * t - g * t * t / 2;
    }

    private float CalculateX(float t)
    {
        return v0 * Mathf.Cos(angle) * t;
    }

    public float CalculateRealX(float t)
    {
        return (initPosition + new Vector3(xDirection * CalculateX(t), CalculateY(time))).x;
    }

    private void DestroyCheck()
    {
        if (yDirection > 0)
        {
            return;
        }
        if (prevY + initPosition.y < groundY)
        {
            Explode();
        }
    }

    private void Explode()
    {
        GameObject explotionInstance = Instantiate(explotion);
        explotionInstance.transform.position = transform.position;
        Camera.main.GetComponent<CameraShake>().Shake();
        SoundManager.instance.Play(sound);
        Destroy(gameObject);
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (yDirection > 0)
        {
            return;
        }
        DamageReceiver damageReceiver = collision.GetComponent<DamageReceiver>();
        if (damageReceiver == null)
        {
            return;
        }
        if (damageReceiver.gameObject.layer != targetLayer)
        {
            return;
        }
        
        damageReceiver.ReceiveDamage(damage);
        Explode();
    }

    private float CalculateAngle(Vector2 target) 
    {
        float d = Mathf.Pow(v0, 4) - g * (g * Mathf.Pow(target.x, 2) + 2 * Mathf.Pow(v0, 2) * target.y);
        if (d < 0)
        {
            throw new ArgumentOutOfRangeException("Target is unreachable");   
        }
        float sqrtd = Mathf.Sqrt(d);
        float t1 = (Mathf.Pow(v0, 2) + sqrtd) / (g * Mathf.Abs(target.x));
        float t2 = (Mathf.Pow(v0, 2) - sqrtd) / (g * Mathf.Abs(target.x));
        float angle1 = Mathf.Atan(t1);
        float angle2 = Mathf.Atan(t2);
        float resultAngle = Mathf.Max(angle1, angle2);
        return resultAngle;
    }

    //threat

    public float CalculateFallTime(float y)
    {
        float d = Mathf.Pow(v0 * Mathf.Sin(angle), 2) - 2 * g * (y - initPosition.y);
        float result = (v0 * Mathf.Sin(angle) + Mathf.Sqrt(d)) / g;
        return result;
    }

    public float GetDamage()
    {
        return damage;
    }
}
