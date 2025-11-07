using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mortair : CarriagePayload
{
    [SerializeField] private List<Sprite> sprites;
    [SerializeField] private ReloadBar reloadBar;
    [SerializeField] private LineAim aim;
    [SerializeField] private Projectile projectilePrefab;
    [SerializeField] private float angleAmp;
    [SerializeField] private SpriteRenderer view;
    [SerializeField] private AudioClip shotSound;
    [SerializeField] private float reloadTime;

    private float gunY = 0.5f;

    public void Start()
    {
        reloadBar.SetReady();
    }
    public void PrepareAttack(Vector2 aimPosition)
    {
        if (!reloadBar.IsReady())
        {
            return;
        }
        aim.Apply(aimPosition);
        SetCanonDirection(Mathf.Abs(aimPosition.x) < 0.3 ? 0 : (int)Mathf.Sign(aimPosition.x));
    }

    public void PerformAttack(Vector2 aimPosition)
    {
        if (!reloadBar.IsReady())
        {
            return;
        }
        if (aimPosition.y < 1.0f)
        {
            aim.Reset();
        }
        else
        {
            aim.InstantClose();
            reloadBar.Reload(reloadTime);
            SoundManager.instance.Play(shotSound);
            Projectile projectile = Instantiate(projectilePrefab);
            projectile.transform.position = transform.position + Vector3.up * gunY;
            projectile.Init(GetAngle(aimPosition.x), ProjectileManager.instance.enemyGround.position.y);
        }
    }

    private void SetCanonDirection(int direction)
    {
        view.sprite = sprites[direction + 1];
    }

    //Angle from vertical line
    private float GetAngle(float aimPositionX)
    {
        return Mathf.Lerp(0.0f, angleAmp, Mathf.Abs(aimPositionX)) * Mathf.Sign(aimPositionX);
    }
}
