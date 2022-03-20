using System;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;

public class LavaObstacle : IDebuff
{
    private readonly LavaConfigurationProvider _configuration;
    private readonly int _level;

    private bool _isActive;
    private Enemy _enemy;

    private bool IsNotPaused => ProjectContext.Instance.PauseManager.IsPaused == false;
    
    public LavaObstacle(LavaConfigurationProvider configuration, int level)
    {
        _configuration = configuration;
        _level = level;
    }
    
    public void Assign(Enemy enemy)
    {
        _enemy = enemy;
        _isActive = true;
        DamageTask();
    }

    public void Remove()
    {
        _isActive = false;
    }

    private async UniTask DamageTask()
    {
        while (_isActive)
        {
            if (IsNotPaused)
            {
                var damage = _configuration.GetDamage(_level);
                _enemy.TakeDamage(damage);
            }
            await Task.Delay(TimeSpan.FromSeconds(0.1f));
        }
    }
}