using System.Collections;
using System.Collections.Generic;
using Core.Building;
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
    private TilesBuilder _tilesBuilder;

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
        _tilesBuilder.Initialize(_contentFactory, _camera, _board);
        BeginNewGame();
    }

    private void OnPauseClicked(bool isPaused)
    {
        Time.timeScale = isPaused ? 0f : 1f;
    }

    private void OnQuitGame()
    {
        GoToMainMenu();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            BeginNewGame();
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
        _tilesBuilder.Enable();
        PlayerHealth = _startingPlayerHealth;
        _prepare = StartCoroutine(PrepareRoutine());
    }

    public void Cleanup()
    {
        _tilesBuilder.Disable();
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
