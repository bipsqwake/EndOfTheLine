using UnityEngine;

public class ArmorCart : CarriagePayload
{
    [SerializeField] private SpriteAim aim;
    [SerializeField] private Shield shield;
    public void PrepareAttack(Vector2 aimPosition)
    {
        aim.Apply(aimPosition);
    }

    public void PerformAttack(Vector2 aimPosition)
    {
        if (aimPosition.y < 1.0f)
        {
            aim.Reset();
        }
        else
        {
            aim.InstantClose();
            shield.Activate(3f, 2.5f);
        }
    }
}
