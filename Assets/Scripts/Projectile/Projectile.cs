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
    public void Init(float angle, float groundY)
    {
        this.initPosition = transform.position;
        this.angle = Mathf.Deg2Rad * (90.0f - Mathf.Abs(angle));
        this.xDirection = (int)Mathf.Sign(angle);
        this.yDirection = 1;
        this.prevY = transform.position.y;
        this.groundY = groundY;
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
        transform.position = initPosition + new Vector3(xDirection * CalculateX(time), CalculateY(time));
        DestroyCheck();
    }

    private float CalculateY(float t)
    {
        return v0 * Mathf.Sin(angle) * t - g / 2 * t * t;
    }

    private float CalculateX(float t)
    {
        return v0 * Mathf.Cos(angle) * t;
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
        damageReceiver.ReceiveDamage(damage);
        Explode();
    }
}
