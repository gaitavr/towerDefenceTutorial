using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Game : MonoBehaviour
{
    [SerializeField]
    private Vector2Int _boardSize;

    [SerializeField]
    private GameBoard _board;

    [SerializeField]
    private Camera _camera;

    [SerializeField]
    private GameTileContentFactory _contentFactory;

    [SerializeField]
    private EnemyFactory _enemyFactory;

    [SerializeField, Range(0.1f, 10f)]
    private float _spawnSpeed;

    private float _spawnProgress;

    private EnemyCollection _enemies = new EnemyCollection();

    private Ray TouchRay => _camera.ScreenPointToRay(Input.mousePosition);

    private void Start()
    {
        _board.Initialize(_boardSize, _contentFactory);
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            HandleTouch();
        }
        else if (Input.GetMouseButtonDown(1))
        {
            HandleAlternativeTouch();
        }

        _spawnProgress += _spawnSpeed * Time.deltaTime;
        while (_spawnProgress >= 1f)
        {
            _spawnProgress -= 1f;
            SpawnEnemy();
        }

        _enemies.GameUpdate();
        Physics.SyncTransforms();
        _board.GameUpdate();
    }

    private void SpawnEnemy()
    {
        GameTile spawnPoint = _board.GetSpawnPoint(Random.Range(0, _board.SpawnPointCount));
        Enemy enemy = _enemyFactory.Get();
        enemy.SpawnOn(spawnPoint);
        _enemies.Add(enemy);
    }
    
    private void HandleTouch()
    {
        GameTile tile = _board.GetTile(TouchRay);
        if (tile != null)
        {
            if (Input.GetKey(KeyCode.LeftShift))
            {
                _board.ToggleTower(tile);
            }
            else
            {
                _board.ToggleWall(tile);
            }
        }
    }

    private void HandleAlternativeTouch()
    {
        GameTile tile = _board.GetTile(TouchRay);
        if (tile != null)
        {
            if (Input.GetKey(KeyCode.LeftShift))
            {
                _board.ToggleDestination(tile);
            }
            else
            {
                _board.ToggleSpawnPoint(tile);
            }
        }
    }
}
