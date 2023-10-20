

namespace Assets.Game.Gameplay.Attack.Common
{
    public sealed class EnemyContext
    {
        public float Scale { get; private set; }
        public float PathOffset { get; private set; }
        public float Speed { get; private set; }
        public float Health { get; private set; }
        public int Damage { get; private set; }

        public EnemyContext(float scale, float pathOffset, float speed, float health, int damage)
        {
            Scale = scale;
            PathOffset = pathOffset;
            Speed = speed;
            Health = health;
            Damage = damage;
        }
    }
}
