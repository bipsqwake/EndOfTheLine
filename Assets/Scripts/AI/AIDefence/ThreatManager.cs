using System;

public sealed class ThreatManager
{
    private ThreatManager() {}

    public static event Action<Projectile> registerProjectileThreat;

    public static void RegisterProjectileThreat(Projectile projectile)
    {
        registerProjectileThreat.Invoke(projectile);
    }
    

}
