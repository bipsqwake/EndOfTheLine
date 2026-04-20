using UnityEngine;

public class Locomotive : Actor
{
    [SerializeField] private Collider2D locomotiveCollider;
    
    public override void SetPlayerControl(bool playerControl)
    {
        base.SetPlayerControl(playerControl);
        locomotiveCollider.gameObject.layer = playerControl ? GlobalSettings.instance.playerLayer : GlobalSettings.instance.enemyLayer;
    }
}
