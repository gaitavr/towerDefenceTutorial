using System.Collections.Generic;
using System.Linq;
using Core;
using UnityEngine;
using Utils.Serialization;

namespace GamePlay.Defend
{

    public class GameBoard : MonoBehaviour
    {
        [SerializeField] private GameTile _tilePrefab;

        private GameTile[] _tiles;

        private readonly Queue<GameTile> _searchFrontier = new();

        private GameTileContentFactory ContentFactory => SceneContext.I.ContentFactory;

        private readonly List<GameTile> _spawnPoints = new();
        private readonly List<GameTileContent> _contentToUpdate = new();

        private UserBoardState _boardData;
        public byte X => _boardData.X;
        public byte Y => _boardData.Y;

        public void Initialize(UserBoardState boardData)
        {
            _boardData = boardData;
            var offset = new Vector2((X - 1) * 0.5f, (Y - 1) * 0.5f);

            _tiles = new GameTile[X * Y];

            for (int i = 0, y = 0; y < Y; y++)
            {
                for (int x = 0; x < X; x++, i++)
                {
                    var tile = _tiles[i] = Instantiate(_tilePrefab);
                    tile.BoardPosition = new Vector2Int(x, y);
                    tile.transform.SetParent(transform, false);
                    tile.transform.localPosition = new Vector3(x - offset.x, 0f, y - offset.y);

                    if (x > 0)
                        GameTile.MakeEastWestNeighbors(tile, _tiles[i - 1]);

                    if (y > 0)
                        GameTile.MakeNorthSouthNeighbors(tile, _tiles[i - X]);

                    tile.IsAlternative = (x & 1) == 0;
                    if ((y & 1) == 0)
                    {
                        tile.IsAlternative = tile.IsAlternative == false;
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

        private bool FindPaths()
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
                var tile = _searchFrontier.Dequeue();
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
                if (t.HasPath == false)
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

            if (content.Type == GameTileContentType.SpawnPoint)
                _spawnPoints.Add(tile);
        }

        public bool TryBuild(GameTile tile, GameTileContent content)
        {
            if (tile.Content.Type != GameTileContentType.Empty)
                return false;

            tile.Content = content;
            if (FindPaths() == false)
            {
                tile.Content = ContentFactory.Get(GameTileContentType.Empty);
                return false;
            }

            _contentToUpdate.Add(content);

            if (content.Type == GameTileContentType.SpawnPoint)
                _spawnPoints.Add(tile);

            return true;
        }

        public void DestroyTile(GameTile tile)
        {
            if (tile.Content.Type <= GameTileContentType.Empty)
                return;

            _contentToUpdate.Remove(tile.Content);

            if (tile.Content.Type == GameTileContentType.SpawnPoint)
                _spawnPoints.Remove(tile);

            tile.Content = ContentFactory.Get(GameTileContentType.Empty);
            FindPaths();
        }

        public GameTile GetTile(Vector3 position)
        {
            var x = (int)(position.x + X * 0.5f);
            var y = (int)(position.z + Y * 0.5f);
            return GetTile(x, y);
        }

        private GameTile GetTile(int x, int y)
        {
            if (x >= 0 && x < X && y >= 0 && y < Y)
                return _tiles[x + y * X];
            return null;
        }

        public IEnumerable<GameTile> GetTilesAround(GameTile tile)
        {
            var result = new List<GameTile>(4)
        {
            GetTile(tile.BoardPosition.x, tile.BoardPosition.y + 1),
            GetTile(tile.BoardPosition.x, tile.BoardPosition.y - 1),
            GetTile(tile.BoardPosition.x + 1, tile.BoardPosition.y),
            GetTile(tile.BoardPosition.x - 1, tile.BoardPosition.y),
        };
            return result.Where(t => t != null);
        }

        public GameTile GetRandomSpawnPoint()
        {
            return _spawnPoints[Random.Range(0, _spawnPoints.Count)];
        }

        public void Clear()
        {
            _spawnPoints.Clear();
            _contentToUpdate.Clear();

            for (var i = 0; i < _boardData.Content.Length; i++)
            {
                ForceBuild(_tiles[i], ContentFactory.Get(_boardData.Content[i], _boardData.Levels[i]));
            }

            FindPaths();
        }

        public GameTileContentType[] GetAllContentTypes => _tiles.Select(t => t.Content.Type).ToArray();
        public byte[] GetAllContentLevels => _tiles.Select(t => (byte)t.Content.Level).ToArray();
    }
}
