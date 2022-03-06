﻿using UnityEngine;

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
        switch (type)
        {
            case GameTileContentType.Destination:
                return Get(_destinationPrefab);
            case GameTileContentType.Empty:
                return Get(_emptyPrefab);
            case GameTileContentType.Wall:
                return Get(_wallPrefab);
            case GameTileContentType.SpawnPoint:
                return Get(_spawnPrefab);
            case GameTileContentType.LaserTower:
                return Get(_laserTower);
            case GameTileContentType.MortarTower:
                return Get(_mortarTower);
            case GameTileContentType.Ice:
                return Get(_iceObstacle);
            case GameTileContentType.Lava:
                return Get(_fireObstacle);
        }

        return null;
    }
    
    private T Get<T>(T prefab) where T : GameTileContent
    {
        T instance = CreateGameObjectInstance(prefab);
        instance.OriginFactory = this;
        return instance;
    }
}
