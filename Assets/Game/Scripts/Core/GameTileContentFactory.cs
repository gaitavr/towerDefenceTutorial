using UnityEngine;

[CreateAssetMenu]
public class GameTileContentFactory : GameObjectFactory
{
    [SerializeField] private GameTileContent _destinationPrefab;
    [SerializeField] private GameTileContent _emptyPrefab;
    [SerializeField] private GameTileContent _wallPrefab;
    [SerializeField] private GameTileContent _spawnPrefab;
    [SerializeField] private Tower[] _laserTowers;
    [SerializeField] private Tower _mortarTower;
    [SerializeField] private GameTileContent _iceObstacle;
    [SerializeField] private GameTileContent _fireObstacle;

    public void Reclaim(GameTileContent content)
    {
        Destroy(content.gameObject);
    }

    private static int _test;

    public GameTileContent Get(GameTileContentType type, int level = 0)
    {
        level = _test;
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
                _test++;
                _test = Mathf.Clamp(_test, 0, 3);
                return Get(_laserTowers[level], level);
            case GameTileContentType.MortarTower:
                return Get(_mortarTower, level);
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
        instance.OriginFactory = this;
        instance.Level = level;
        return instance;
    }
}
