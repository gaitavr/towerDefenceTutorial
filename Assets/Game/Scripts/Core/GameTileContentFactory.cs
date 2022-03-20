using Game.Defend.MortarTower;
using UnityEngine;

[CreateAssetMenu]
public class GameTileContentFactory : GameObjectFactory
{
    [SerializeField] private GameTileContent _destinationPrefab;
    [SerializeField] private GameTileContent _emptyPrefab;
    [SerializeField] private GameTileContent _wallPrefab;
    [SerializeField] private GameTileContent _spawnPrefab;
    [SerializeField] private Tower[] _laserTowers;
    [SerializeField] private Tower[] _mortarTowers;
    [SerializeField] private GameTileContent _iceObstacle;
    [SerializeField] private GameTileContent _fireObstacle;

    public LaserTowerConfigurationProvider LaserConfig { get; private set; } = new LaserTowerConfigurationProvider();
    public MortarTowerConfigurationProvider MortarConfig { get; private set; } = new MortarTowerConfigurationProvider();
    
    public void Reclaim(GameTileContent content)
    {
        Destroy(content.gameObject);
    }

    public GameTileContent Get(GameTileContentType type, int level = 0)
    {
        switch (type)
        {
            case GameTileContentType.Destination:
                return Get(_destinationPrefab, level);
            case GameTileContentType.Empty:
                return Get(_emptyPrefab, level);
            case GameTileContentType.Wall:
                return Get(_wallPrefab, level);
            case GameTileContentType.SpawnPoint:
                return Get(_spawnPrefab, level);
            case GameTileContentType.LaserTower:
                if (level >= _laserTowers.Length)
                    level = _laserTowers.Length - 1;
                return Get(_laserTowers[level], level);
            case GameTileContentType.MortarTower:
                if (level >= _mortarTowers.Length)
                    level = _mortarTowers.Length - 1;
                return Get(_mortarTowers[level], level);
            case GameTileContentType.Ice:
                return Get(_iceObstacle, level);
            case GameTileContentType.Lava:
                return Get(_fireObstacle, level);
        }

        return null;
    }
    
    private T Get<T>(T prefab, int level) where T : GameTileContent
    {
        T instance = CreateGameObjectInstance(prefab);
        instance.Initialize(this, level);
        return instance;
    }
}
