using UnityEngine;

[CreateAssetMenu(fileName = "AIArmorCart", menuName = "AICarriage/AIArmorCart")]
public class AIArmorCart : AICarriage
{
    [SerializeField] private float duration;
    [SerializeField] private float reloadTime;
    [SerializeField] private float shieldWidth;

    private float lastUse;

    public override void Init()
    {
        lastUse = 0.0f - reloadTime - 100f; //I know thats kinda lame
    }
    public override AIActionType[] GetActionType()
    {
        return new AIActionType[] { AIActionType.REACTION };
    }

    public float GetReloadTimeLeft()
    {
        return Mathf.Max(reloadTime - (Time.time - lastUse), 0.0f);
    }

    public float GetActiveTimeLeft()
    {
        return Mathf.Max(0.0f, duration - (Time.time - lastUse));
    }

    public float GetShieldWidth()
    {
        return shieldWidth;
    }

    public void Activate()
    {
        if (!instance.IsCarriageDestroyed())
        {
            ((ArmorCart)instance.GetCarriagePayload()).PerformAttack(duration, shieldWidth);   
            lastUse = Time.time;
        }
    }

}