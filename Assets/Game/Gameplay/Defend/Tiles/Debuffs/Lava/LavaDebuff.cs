using System;
using System.Threading;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using GamePlay.Attack;

namespace GamePlay.Defend
{
    public class LavaDebuff : IDebuff
    {
        private readonly LavaConfigurationProvider _configuration;
        private readonly int _level;

        private Enemy _enemy;
        private CancellationTokenSource _cancellationTokenSource;

        private bool IsNotPaused => ProjectContext.I.PauseManager.IsPaused == false;

        public LavaDebuff(LavaConfigurationProvider configuration, int level)
        {
            _configuration = configuration;
            _level = level;
        }

        public void Assign(Enemy enemy)
        {
            _enemy = enemy;
            _cancellationTokenSource = new CancellationTokenSource();
            DamageTask(_cancellationTokenSource.Token).Forget();
        }

        public void Remove()
        {
            _cancellationTokenSource?.Cancel();
        }

        private async UniTask DamageTask(CancellationToken cancellationToken)
        {
            var damage = _configuration.GetDamage(_level);
            var delay = _configuration.GetDelay(_level);
            while (cancellationToken.IsCancellationRequested == false)
            {
                if (IsNotPaused)
                    _enemy.TakeDamage(damage);
                await Task.Delay(TimeSpan.FromSeconds(delay), cancellationToken);
            }
        }
    }
}