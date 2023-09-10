using GamePlay.Attack;

namespace MainMenu
{
    public interface IAttackScenarioViewController
    {
        void SetNewEnemyCount(EnemyType enemyType, int newCount);
    }
}