public class LaserTowerConfigurationProvider
{
    public float GetDamagePerSecond(int level)
    {
        return (level + 1) * 10;
    }
}