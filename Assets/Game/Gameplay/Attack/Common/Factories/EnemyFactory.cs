
namespace GamePlay.Attack
{
    public abstract class EnemyFactory : GameObjectFactory
    {
        public Enemy Get(EnemyType type)
        {
            var config = GetConfig(type);
            Enemy instance = CreateGameObjectInstance(config.Prefab);
            instance.OriginFactory = this;
            instance.Initialize(config.ToContext());
            return instance;
        }

        protected abstract EnemyConfig GetConfig(EnemyType type);

        public void Reclaim(Enemy enemy)
        {
            Destroy(enemy.gameObject);
        }
    }
}
