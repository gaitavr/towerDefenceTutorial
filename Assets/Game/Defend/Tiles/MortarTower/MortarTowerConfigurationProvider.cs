public class MortarTowerConfigurationProvider
{
    public float GetShootsPerSecond(int level)
    {
        return level + 1;
    }
}