using UnityEngine;

[CreateAssetMenu]
public class GameTileContentFactory : GameObjectFactory
{
    [SerializeField] private GameTileContent _destinationPrefab;
    [SerializeField] private GameTileContent _emptyPrefab;
    [SerializeField] private GameTileContent _wallPrefab;
    [SerializeField] private GameTileContent _spawnPrefab;
    [SerializeField] private Tower _laserTower;
    [SerializeField] private Tower _mortarTower;
    [SerializeField] private GameTileContent _iceObstacle;
    [SerializeField] private GameTileContent _fireObstacle;

    public void Reclaim(GameTileContent content)
    {
        Destroy(content.gameObject);
    }

    public GameTileContent Get(GameTileContentType type)
    {
        int level = Random.Range(0, 5);
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
                return Get(_laserTower, level);
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
