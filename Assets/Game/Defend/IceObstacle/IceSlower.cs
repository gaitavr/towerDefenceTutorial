using UnityEngine;

public class IceSlower : IDebuff
{
    private readonly IceConfigurationProvider _configuration;
    private readonly int _level;

    public IceSlower(IceConfigurationProvider configuration, int level)
    {
        _configuration = configuration;
        _level = level;
    }
    
    public void Assign(Enemy enemy)
    {
        var slow = _configuration.GetSlow(_level);
        enemy.SetSpeed(slow);
    }

    public void Delete(Enemy enemy)
    {
        enemy.SetSpeed(1f);
    }
}
