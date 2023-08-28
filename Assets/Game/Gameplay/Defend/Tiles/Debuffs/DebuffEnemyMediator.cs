using GamePlay.Attack;
using System;

namespace GamePlay.Defend
{
    public class DebuffEnemyMediator
    {
        private readonly Enemy _enemy;
        private IDebuff _currentDebuff;

        public DebuffEnemyMediator(Enemy enemy)
        {
            _enemy = enemy;
        }

        public void Replace(IDebuff debuff)
        {
            _currentDebuff?.Remove();
            _currentDebuff = debuff;
            _currentDebuff.Assign(_enemy);
        }

        public void Dispose()
        {
            _currentDebuff?.Remove();
        }
    }
}