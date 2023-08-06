using UnityEngine;

public class LavaConfigurationProvider
{
    public float GetDamage(int level)
    {
        return level * 2f + 1;
    }

    public float GetDelay(int level)
    {
        var delay = 0.25f - level * 0.1f;
        return Mathf.Max(0.05f, delay);
    }
}