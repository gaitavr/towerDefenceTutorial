using GamePlay.Defend;

namespace GamePlay.Modes
{
    public interface IGameEntityInteructionProxy
    {
        Shell SpawnShell();

        Explosion SpawnExplosion();

        void EnemyReachedDestination(int damage);
    }
}
