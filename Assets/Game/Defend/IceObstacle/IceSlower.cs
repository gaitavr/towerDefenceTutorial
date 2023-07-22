
public class IceSlower : IDebuff
{
    private readonly IceConfigurationProvider _configuration;
    private readonly int _level;

    private Enemy _enemy;

    public IceSlower(IceConfigurationProvider configuration, int level)
    {
        _configuration = configuration;
        _level = level;
    }
    
    public void Assign(Enemy enemy)
    {
        _enemy = enemy;
        var slow = _configuration.GetSlow(_level);
        enemy.SetSpeed(slow);
    }

    public void Remove()
    {
        _enemy.SetSpeed(1f);
    }
}
