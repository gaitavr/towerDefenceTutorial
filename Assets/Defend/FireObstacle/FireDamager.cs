using System;
using System.Threading.Tasks;

public class FireDamager : IDebuff
{
    private const int DAMAGE = 1;

    private bool _isActive;
    private Enemy _enemy;
    
    public void Assign(Enemy enemy)
    {
        _enemy = enemy;
        _isActive = true;
        DamageTask();
    }

    public void Delete(Enemy enemy)
    {
        _isActive = false;
    }

    private async Task DamageTask()
    {
        while (_isActive)
        {
            _enemy.TakeDamage(DAMAGE);
            await Task.Delay(TimeSpan.FromSeconds(0.1f));
        }
    }
}