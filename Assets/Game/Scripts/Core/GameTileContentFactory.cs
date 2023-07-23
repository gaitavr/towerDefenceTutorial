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
    [SerializeField] private GameTileContent[] _iceObstacles;
    [SerializeField] private GameTileContent[] _lavaObstacles;

    public LaserTowerConfigurationProvider LaserConfig { get; } = new();
    public MortarTowerConfigurationProvider MortarConfig { get; } = new();
    public IceConfigurationProvider IceConfig { get; } = new();
    public LavaConfigurationProvider LavaConfig { get; } = new();
    
    public void Reclaim(GameTileContent content)
    {
        Destroy(content.gameObject);
    }

    public bool IsNextUpgradeAllowed(GameTileContent gameTileContent)
    {
        var nextLevel = gameTileContent.Level + 1;
        switch (gameTileContent.Type)
        {
            case GameTileContentType.Destination:
            case GameTileContentType.Empty:
            case GameTileContentType.Wall:
            case GameTileContentType.SpawnPoint:
                return false;
            case GameTileContentType.LaserTower:
                return _laserTowers.Length > nextLevel;
            case GameTileContentType.MortarTower:
                return _mortarTowers.Length > nextLevel;
            case GameTileContentType.Ice:
                return _iceObstacles.Length > nextLevel;
            case GameTileContentType.Lava:
                return _lavaObstacles.Length > nextLevel;
        }

        return false;
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
                if (level >= _iceObstacles.Length)
                    level = _iceObstacles.Length - 1;
                return Get(_iceObstacles[level], level);
            case GameTileContentType.Lava:
                if (level >= _lavaObstacles.Length)
                    level = _lavaObstacles.Length - 1;
                return Get(_lavaObstacles[level], level);
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
