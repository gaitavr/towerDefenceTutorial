public class LavaConfigurationProvider
{
    public float GetDamage(int level)
    {
        return level * 2f + 1;
    }
}