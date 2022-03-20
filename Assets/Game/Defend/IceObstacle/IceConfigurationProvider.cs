public class IceConfigurationProvider
{
    public float GetSlow(int level)
    {
        return 1f / (level + 1.25f);
    }
}