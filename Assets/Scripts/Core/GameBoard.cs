using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Serialization;
using UnityEngine;

public class GameBoard : MonoBehaviour
{
    [SerializeField]
    private GameTile _tilePrefab;

    private Vector2Int _size;

    private GameTile[] _tiles;

    private Queue<GameTile> _searchFrontier = new Queue<GameTile>();

    private GameTileContentFactory _contentFactory;

    private List<GameTile> _spawnPoints = new List<GameTile>();

    public int SpawnPointCount => _spawnPoints.Count;

    private List<GameTileContent> _contentToUpdate = new List<GameTileContent>();

    private readonly BoardSerializer _serializer = new BoardSerializer();
    
    public void Initialize(Vector2Int size, GameTileContentFactory contentFactory)
    {
        _size = size;
        TryLoad();
        Vector2 offset = new Vector2((size.x - 1) * 0.5f, (size.y - 1) * 0.5f);

        _tiles = new GameTile[size.x * size.y];
        _contentFactory = contentFactory;
        for (int i = 0, y = 0; y < size.y; y++)
        {
            for (int x = 0; x < size.x; x++, i++)
            {
                GameTile tile = _tiles[i] = Instantiate(_tilePrefab);
                tile.transform.SetParent(transform, false);
                tile.transform.localPosition = new Vector3(x - offset.x, 0f, y - offset.y);

                if (x > 0)
                {
                    GameTile.MakeEastWestNeighbors(tile, _tiles[i - 1]);
                }

                if (y > 0)
                {
                    GameTile.MakeNorthSouthNeighbors(tile, _tiles[i - size.x]);
                }

                tile.IsAlternative = (x & 1) == 0;
                if ((y & 1) == 0)
                {
                    tile.IsAlternative = !tile.IsAlternative;
                }
            }
        }

        Clear();
    }

    public void GameUpdate()
    {
        for (int i = 0; i < _contentToUpdate.Count; i++)
        {
            _contentToUpdate[i].GameUpdate();
        }
    }

    public bool FindPaths()
    {
        foreach (var t in _tiles)
        {
            if (t.Content.Type == GameTileContentType.Destination)
            {
                t.BecomeDestination();
                _searchFrontier.Enqueue(t);
            }
            else
            {
                t.ClearPath();
            }
        }

        if (_searchFrontier.Count == 0)
        {
            return false;
        }

        while (_searchFrontier.Count > 0)
        {
            GameTile tile = _searchFrontier.Dequeue();
            if (tile != null)
            {
                if (tile.IsAlternative)
                {
                    _searchFrontier.Enqueue(tile.GrowPathNorth());
                    _searchFrontier.Enqueue(tile.GrowPathSouth());
                    _searchFrontier.Enqueue(tile.GrowPathEast());
                    _searchFrontier.Enqueue(tile.GrowPathWest());
                }
                else
                {
                    _searchFrontier.Enqueue(tile.GrowPathWest());
                    _searchFrontier.Enqueue(tile.GrowPathEast());
                    _searchFrontier.Enqueue(tile.GrowPathSouth());
                    _searchFrontier.Enqueue(tile.GrowPathNorth());
                }
            }
        }

        foreach (var t in _tiles)
        {
            if (!t.HasPath)
            {
                return false;
            }
        }

        foreach (var t in _tiles)
        {
            t.ShowPath();
        }

        return true;
    }

    public void ForceBuild(GameTile tile, GameTileContent content)
    {
        tile.Content = content;
        _contentToUpdate.Add(content);
        
        if(content.Type == GameTileContentType.SpawnPoint)
            _spawnPoints.Add(tile);
    }
    
    public bool TryBuild(GameTile tile, GameTileContent content)
    {
        if (tile.Content.Type != GameTileContentType.Empty)
            return false;

        tile.Content = content;
        if (FindPaths() == false)
        {
            tile.Content = _contentFactory.Get(GameTileContentType.Empty);
            return false;
        }
        
        _contentToUpdate.Add(content);
        
        if(content.Type == GameTileContentType.SpawnPoint)
            _spawnPoints.Add(tile);
        
        return true;
    }

    private void ClearTile(GameTile tile)
    {
        if (tile.Content.Type <= GameTileContentType.Empty)
            return;
        
        _contentToUpdate.Remove(tile.Content);
        tile.Content = _contentFactory.Get(GameTileContentType.Empty);
        FindPaths();
    }
    
    public GameTile GetTile(Ray ray)
    {
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, float.MaxValue, 1))
        {
            int x = (int) (hit.point.x + _size.x * 0.5f);
            int y = (int) (hit.point.z + _size.y * 0.5f);
            if (x >= 0 && x < _size.x && y >= 0 && y < _size.y)
            {
                return _tiles[x + y * _size.x];
            }
        }

        return null;
    }

    public GameTile GetSpawnPoint(int index)
    {
        return _spawnPoints[index];
    }

    public void Clear()
    {
        foreach (GameTile tile in _tiles)
        {
            tile.Content = _contentFactory.Get(GameTileContentType.Empty);
        }
        _spawnPoints.Clear();
        _contentToUpdate.Clear();

        for (int i = 0; i < _boardData.Content.Length; i++)
        {
            ForceBuild(_tiles[i], _contentFactory.Get(_boardData.Content[i]));
        }

        FindPaths();

        // TryBuild(_tiles[_tiles.Length / 2], _contentFactory.Get(GameTileContentType.Destination));
        // TryBuild(_tiles[0], _contentFactory.Get(GameTileContentType.SpawnPoint));
    }

    private void Update()
    {
        if (Input.GetKeyUp(KeyCode.Space))
        {
            var data = new BoardData()
            {
                Version = Serialization.Serialization.VERSION,
                AccountId = 1145,
                X = (byte)_size.x,
                Y = (byte)_size.y,
                Content = _tiles.Select(t => t.Content.Type).ToArray()
            };
            _serializer.Save(data);
        }
    }

    private BoardData _boardData;
    
    private void TryLoad()
    {
        _boardData = _serializer.Load();
        if(_boardData == null)
            return;
        _size = new Vector2Int(_boardData.X, _boardData.Y);
        
    }
}
