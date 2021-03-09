using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Core.UI;
using GameResult;
using Loading;
using UnityEngine;
using Common;

public class QuickGame : MonoBehaviour, ICleanUp
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
    private GameResultWindow _gameResultWindow;

    [SerializeField]
    private PrepareGamePanel _prepareGamePanel;

    [SerializeField]
    private Camera _camera;

    [SerializeField]
    private GameTileContentFactory _contentFactory;

    [SerializeField]
    private WarFactory _warFactory;
    
    [SerializeField]
    private EnemyFactory _enemyFactory;

    [SerializeField]
    private GameScenario _scenario;

    [SerializeField, Range(0, 100)]
    private int _startingPlayerHealth = 10;

    [SerializeField, Range(5f, 30f)]
    private float _prepareTime = 15f;

    private bool _scenarioInProcess;
    private GameScenario.State _activeScenario;
    private CancellationTokenSource _prepareCancellation;

    private readonly GameBehaviorCollection _enemies = new GameBehaviorCollection();
    private readonly GameBehaviorCollection _nonEnemies = new GameBehaviorCollection();

    private static QuickGame _instance;

    private int _playerHealth;
    private int PlayerHealth
    {
        get => _playerHealth;
        set
        {
            _playerHealth = Mathf.Max(0, value);
            _defenderHud.UpdatePlayerHealth(_playerHealth, _startingPlayerHealth);
        }
    }
    
    public IEnumerable<GameObjectFactory> Factories => new GameObjectFactory[]{_contentFactory, 
        _warFactory, _enemyFactory};

    public string SceneName => Constants.Scenes.QUICK_GAME;
    
    private void OnEnable()
    {
        _instance = this;
    }

    public void Init()
    {
        _defenderHud.PauseClicked += OnPauseClicked;
        _defenderHud.QuitGame += GoToMainMenu;
        var initialData = GenerateInitialData();
        _board.Initialize(initialData, _contentFactory);
        _tilesBuilder.Initialize(_contentFactory, _camera, _board, false);
    }

    private BoardData GenerateInitialData()
    {
        var result = new BoardData
        {
            X = (byte) _boardSize.x,
            Y = (byte) _boardSize.y,
            Content = new GameTileContentType[_boardSize.x * _boardSize.y]
        };
        result.Content[0] = GameTileContentType.SpawnPoint;
        result.Content[result.Content.Length - 1] = GameTileContentType.Destination;
        return result;
    }

    private void OnPauseClicked(bool isPaused)
    {
        Time.timeScale = isPaused ? 0f : 1f;
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
            _defenderHud.UpdateScenarioWaves(waves.currentWave, waves.wavesCount);
            if (PlayerHealth <= 0)
            {
                _scenarioInProcess = false;
                _gameResultWindow.Show(GameResultType.Defeat, BeginNewGame, GoToMainMenu);
            }
            if (_activeScenario.Progress() == false && _enemies.IsEmpty)
            {
                _scenarioInProcess = false;
                _gameResultWindow.Show(GameResultType.Victory, BeginNewGame, GoToMainMenu);
            }
        }

        _enemies.GameUpdate();
        Physics.SyncTransforms();
        _board.GameUpdate();
        _nonEnemies.GameUpdate();
    }

    public static void SpawnEnemy(EnemyFactory factory, EnemyType enemyType)
    {
        var spawnPoint = _instance._board.GetRandomSpawnPoint();
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
    
    public async void BeginNewGame()
    {
        Cleanup();
        _tilesBuilder.Enable();
        PlayerHealth = _startingPlayerHealth;

        try
        {
            _prepareCancellation?.Dispose();
            _prepareCancellation = new CancellationTokenSource();
            if (await _prepareGamePanel.Prepare(_prepareTime, _prepareCancellation.Token))
            {
                _activeScenario = _scenario.Begin();
                _scenarioInProcess = true;
            }
        }
        catch (TaskCanceledException _){}
    }

    public void Cleanup()
    {
        _tilesBuilder.Disable();
        _scenarioInProcess = false;
        _prepareCancellation?.Cancel();
        _prepareCancellation?.Dispose();
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
}
