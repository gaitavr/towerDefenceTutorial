using GamePlay.Defend;

namespace GamePlay.Modes
{
    public interface IEnemyInteructionProxy
    {
        Shell SpawnShell();

        Explosion SpawnExplosion();

        void EnemyReachedDestination(int damage);
    }
}
