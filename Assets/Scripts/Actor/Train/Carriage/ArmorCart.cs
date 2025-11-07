using UnityEngine;

public class ArmorCart : CarriagePayload
{
    [SerializeField] private SpriteAim aim;
    [SerializeField] private Shield shield;
    [SerializeField] private ReloadBar reloadBar;
    [SerializeField] private float reloadTime;

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
            reloadBar.Reload(reloadTime);
            aim.InstantClose();
            shield.Activate(3f, 2.5f);
        }
    }
}
