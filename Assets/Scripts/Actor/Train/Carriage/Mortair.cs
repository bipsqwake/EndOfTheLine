using System;
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
    private MortairConfiguration playerConfiguration;
    private AIMortairConfiguration enemyConfiguration;

    private float gunY = 0.5f;

    public void Start()
    {
        reloadBar.SetReady();
    }
    public void PrepareAttack(Vector2 aimPosition)
    {
        if (!State.instance.IsPlayerControl() || !playerControl || !reloadBar.IsReady())
        {
            return;
        }
        aim.Apply(aimPosition);
        SetCanonDirection(Mathf.Abs(aimPosition.x) < 0.3 ? 0 : (int)Mathf.Sign(aimPosition.x));
    }

    //for player attacks
    public void PerformAttack(Vector2 aimPosition)
    {
        if (!State.instance.IsPlayerControl() || !playerControl || !reloadBar.IsReady())
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
            reloadBar.Reload(playerConfiguration.GetReloadTime());
            SoundManager.instance.Play(shotSound);
            Projectile projectile = Instantiate(projectilePrefab);
            projectile.transform.position = transform.position + Vector3.up * gunY;
            projectile.Init(GetAngle(aimPosition.x), ProjectileManager.instance.enemyGround.position.y, GetTargetLayer());
            ThreatManager.RegisterProjectileThreat(projectile);
        }
    }

    //for enemy attacks
    public void PerformTargetAttack(Vector3 target)
    {
        if (playerControl)
        {
            return;
        }
        SoundManager.instance.Play(shotSound);
        Projectile projectile = Instantiate(projectilePrefab);
        projectile.transform.position = transform.position + Vector3.up * gunY;
        //TODO: Add some noise to target location
        projectile.Init(target - projectile.transform.position, ProjectileManager.instance.playerGround.position.y, GetTargetLayer());
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

    public override void SetPlayerControl(bool playerControl)
    {
        base.SetPlayerControl(playerControl);

        reloadBar.SetVisible(playerControl);
    }

    private int GetTargetLayer()
    {
        return playerControl ? GlobalSettings.instance.enemyLayer : GlobalSettings.instance.playerLayer;
    }

    public override void SetConfiguration(TrainPartConfiguration configuration)
    {
        if (configuration.GetType() != typeof(MortairConfiguration))
        {
            return;
        }
        playerConfiguration = (MortairConfiguration) configuration;
        SetInitHealth(playerConfiguration.GetMaxHealth());
        return;
    }

    public override void SetConfiguration(AITrainPartConfiguration configuration)
    {
        if (configuration.GetType() != typeof(AIMortairConfiguration))
        {
            return;
        }
        enemyConfiguration = (AIMortairConfiguration) configuration;
        SetInitHealth(enemyConfiguration.GetMaxHealth());
        return;
    }

    public override AIActionType[] GetActionType()
    {
        return new AIActionType[] {AIActionType.COROUTINE};
    }

    public override IEnumerator CoroutineAction()
    {
        yield return new WaitForSeconds(UnityEngine.Random.Range(enemyConfiguration.GetMinStartDelay(), enemyConfiguration.GetMaxStartDelay()));
        while (IsDestroyed())
        {
            // AttackCycle();
            yield return new WaitForSeconds(UnityEngine.Random.Range(enemyConfiguration.GetMinReloadTime(), enemyConfiguration.GetMaxReloadTime()));
        }
    }

    private void AttackCycle()
    {
        Actor target = SelectTarget();
        Vector3 targetCoord = SelectTargetCoord(target);
        PerformTargetAttack(targetCoord);
    }

    private Actor SelectTarget()
    {
        Dictionary<Actor, int> playerWeigth = PlayerTargetSelectorStrategies.GetSelector(enemyConfiguration.GetStrategy()).GetPlayerTrainWeght();
        int sum = 0;
        foreach (var part in playerWeigth)
        {
            sum += part.Value;
        }
        int rand = UnityEngine.Random.Range(0, sum);
        foreach (var part in playerWeigth)
        {
            if (rand < part.Value)
            {
                return part.Key;
            }
            rand -= part.Value;
        }
        return null;
    }

    private Vector3 SelectTargetCoord(Actor target)
    {
        if (target == null)
        {
            throw new ArgumentNullException("Target should not be null");
        }
        return target.transform.position;
    }

    public override float GetImportance()
    {
        return enemyConfiguration.GetImportance();
    }
}
