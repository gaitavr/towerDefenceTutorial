using System.Collections;
using System.Collections.Generic;
using Core.UI;
using Loading;
using UnityEngine;

public class Game : MonoBehaviour
{
    [SerializeField]
    private Vector2Int _boardSize;

    [SerializeField]
    private GameBoard _board;

    [SerializeField]
    private DefenderHud _defenderHud;

    [SerializeField]
    private Camera _camera;

    [SerializeField]
    private GameTileContentFactory _contentFactory;

    [SerializeField]
    private WarFactory _warFactory;

    [SerializeField]
    private GameScenario _scenario;

    [SerializeField, Range(0, 100)]
    private int _startingPlayerHealth = 10;

    [SerializeField, Range(5f, 30f)]
    private float _prepareTime = 15f;

    private bool _scenarioInProcess;
    private GameScenario.State _activeScenario;

    private GameBehaviorCollection _enemies = new GameBehaviorCollection();
    private GameBehaviorCollection _nonEnemies = new GameBehaviorCollection();

    private Ray TouchRay => _camera.ScreenPointToRay(Input.mousePosition);

    private TowerType _currentTowerType;

    private static Game _instance;

    private int _playerHealth;
    private int PlayerHealth
    {
        get => _playerHealth;
        set
        {
            _playerHealth = value;
            _defenderHud.UpdatePlayerHealth(_playerHealth, _startingPlayerHealth);
        }
    }

    private void OnEnable()
    {
        _instance = this;
    }

    private void Start()
    {
        _defenderHud.PauseClicked += OnPauseClicked;
        _defenderHud.QuitGame += OnQuitGame;
        _board.Initialize(_boardSize, _contentFactory);
        BeginNewGame();
    }

    private void OnPauseClicked(bool isPaused)
    {
        Debug.Log("Paused: " + isPaused);
        Time.timeScale = isPaused ? 0f : 1f;
    }

    private void OnQuitGame()
    {
        Debug.Log("Quit game");
        GoToMainMenu();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            BeginNewGame();
        }

        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            _currentTowerType = TowerType.Laser;
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            _currentTowerType = TowerType.Mortar;
        }

        if (Input.GetMouseButtonDown(0))
        {
            HandleTouch();
        }
        else if (Input.GetMouseButtonDown(1))
        {
            HandleAlternativeTouch();
        }

        if (_scenarioInProcess)
        {
            var waves = _activeScenario.GetWaves();
            _defenderHud.UpdateScenarioWaves(waves.Item1, waves.Item2);
            if (PlayerHealth <= 0)
            {
                Debug.Log("Defeated!");
                BeginNewGame();
            }
            if (!_activeScenario.Progress() && _enemies.IsEmpty)
            {
                Debug.Log("Victory!");
                BeginNewGame();
                _activeScenario.Progress();
            }
        }

        _enemies.GameUpdate();
        Physics.SyncTransforms();
        _board.GameUpdate();
        _nonEnemies.GameUpdate();
    }

    public static void SpawnEnemy(EnemyFactory factory, EnemyType enemyType)
    {
        var spawnPoint = _instance._board.GetSpawnPoint(Random.Range(0, _instance._board.SpawnPointCount));
        var enemy = factory.Get(enemyType);
        enemy.SpawnOn(spawnPoint);
        _instance._enemies.Add(enemy);
    }
    
    private void HandleTouch()
    {
        var tile = _board.GetTile(TouchRay);
        if (tile != null)
        {
            if (Input.GetKey(KeyCode.LeftShift))
            {
                _board.ToggleTower(tile, _currentTowerType);
            }
            else
            {
                _board.ToggleWall(tile);
            }
        }
    }

    private void HandleAlternativeTouch()
    {
        var tile = _board.GetTile(TouchRay);
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

    public static Shell SpawnShell()
    {
        var shell = _instance._warFactory.Shell;
        _instance._nonEnemies.Add(shell);
        return shell;
    }

    public static Explosion SpawnExplosion()
    {
        var shell = _instance._warFactory.Explosion;
        _instance._nonEnemies.Add(shell);
        return shell;
    }

    public static void EnemyReachedDestination()
    {
        _instance.PlayerHealth--;
    }

    private void BeginNewGame()
    {
        Cleanup();
        PlayerHealth = _startingPlayerHealth;
        _prepare = StartCoroutine(PrepareRoutine());
    }

    public void Cleanup()
    {
        _scenarioInProcess = false;
        if (_prepare != null)
        {
            StopCoroutine(_prepare);
        }
        _enemies.Clear();
        _nonEnemies.Clear();
        _board.Clear();
    }

    private void GoToMainMenu()
    {
        var operations = new Queue<ILoadingOperation>();
        operations.Enqueue(new ClearGameOperation(this));
        LoadingScreen.Instance.Load(operations);
    }

    private Coroutine _prepare;
    private IEnumerator PrepareRoutine()
    {
        yield return new WaitForSeconds(_prepareTime);
        _activeScenario = _scenario.Begin();
        _scenarioInProcess = true;
    }
}
