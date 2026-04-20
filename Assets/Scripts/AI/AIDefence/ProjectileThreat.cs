public class ProjectileThreat : Threat
{
    private Projectile projectile;
    private float targetX;

    public ProjectileThreat (Projectile projectile)
    {
        this.projectile = projectile;
    }

    public void SetTargetX(float x)
    {
        this.targetX = x;
    }

    public float GetTargetX()
    {
        return targetX;
    }

    public Projectile GetProjectile()
    {
        return projectile;
    }

    public override bool Finished()
    {
        return projectile == null;
    }
}