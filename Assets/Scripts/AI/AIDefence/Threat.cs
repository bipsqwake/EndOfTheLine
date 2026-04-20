public abstract class Threat
{
    private float tti;

    public void ReduceTTI(float time)
    {
        tti -= time;
    }

    public float GetTTI()
    {
        return tti;
    }

    public void SetTTI(float tti)
    {
        this.tti = tti;
    }

    public abstract bool Finished();
}