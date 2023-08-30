using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Core.UI;
using GameResult;
using Core.Loading;
using UnityEngine;
using Core.Pause;
using Cysharp.Threading.Tasks;
using UnityEngine.ResourceManagement.ResourceProviders;
using Utils.Serialization;
using Core;
using GamePlay.Defend;
using GamePlay.Attack;

namespace GamePlay.Modes
{
    public sealed class QuickGameMode : MonoBehaviour, IGameModeCleaner, IPauseHandler
    {
        [SerializeField] private Vector2Int _boardSize;
        [SerializeField] private DefenderHud _defenderHud;
        [SerializeField] private GameResultWindow _gameResultWindow;
        [SerializeField] private PrepareGamePanel _prepareGamePanel;
        [Space]
        [SerializeField] private GameScenario _scenario;
        [SerializeField, Range(0, 100)] private int _startingPlayerHealth = 10;
        [SerializeField, Range(5f, 30f)] private float _prepareTime = 15f;

        private bool _scenarioInProcess;
        private GameScenario.State _activeScenario;
        private CancellationTokenSource _prepareCancellation;
        private SceneInstance _environment;
        private int _playerHealth;
        private static QuickGameMode _instance;

        private readonly GameBehaviorCollection _enemies = new();
        private readonly GameBehaviorCollection _nonEnemies = new();

        private bool IsPaused => ProjectContext.I.PauseManager.IsPaused;

        private int PlayerHealth
        {
            get => _playerHealth;
            set
            {
                _playerHealth = Mathf.Max(0, value);
                _defenderHud.UpdatePlayerHealth(_playerHealth, _startingPlayerHealth);
            }
        }

        public IEnumerable<GameObjectFactory> Factories => new GameObjectFactory[]
        {
            SceneContext.I.ContentFactory, SceneContext.I.WarFactory,
            SceneContext.I.EnemyFactory
        };

        public string SceneName => Utils.Constants.Scenes.QUICK_GAME_MODE;

        private TilesBuilderViewController TilesBuilder => SceneContext.I.TilesBuilder;
        private GameBoard GameBoard => SceneContext.I.GameBoard;

        public void Init(SceneInstance environment)
        {
            _instance = this;
            ProjectContext.I.PauseManager.Register(this);
            SceneContext.I.Initialize();
            _environment = environment;
            var initialData = UserBoardState.GetInitial(_boardSize);
            GameBoard.Initialize(initialData);
        }

        private void Update()
        {
            if (IsPaused || _instance == null)
                return;

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
            GameBoard.GameUpdate();
            SceneContext.I.GameTileRaycaster.GameUpdate();
            TilesBuilder.GameUpdate();
            _nonEnemies.GameUpdate();
        }

        public async void BeginNewGame()
        {
            Cleanup();
            TilesBuilder.SetActive(true);
            PlayerHealth = _startingPlayerHealth;
            _defenderHud.QuitGame += GoToMainMenu;

            try
            {
                _prepareCancellation?.Dispose();
                _prepareCancellation = new CancellationTokenSource();
                var prepareResult = await _prepareGamePanel.Prepare(_prepareTime, _prepareCancellation.Token);
                if (prepareResult)
                {
                    _activeScenario = _scenario.Begin();
                    _scenarioInProcess = true;
                }
            }
            catch (TaskCanceledException _)
            {

            }
        }

        public void Cleanup()
        {
            _defenderHud.QuitGame -= GoToMainMenu;
            TilesBuilder.SetActive(false);
            _scenarioInProcess = false;
            _prepareCancellation?.Cancel();
            _prepareCancellation?.Dispose();
            _enemies.Clear();
            _nonEnemies.Clear();
            GameBoard.Clear();
        }

        private void GoToMainMenu()
        {
            var operations = new Queue<ILoadingOperation>();
            operations.Enqueue(new ClearGameOperation(this));
            ProjectContext.I.AssetProvider.UnloadAdditiveScene(_environment).Forget();
            ProjectContext.I.LoadingScreenProvider.LoadAndDestroy(operations).Forget();
        }

        public static void SpawnEnemy(EnemyFactory factory, EnemyType enemyType)
        {
            var spawnPoint = _instance.GameBoard.GetRandomSpawnPoint();
            var enemy = factory.Get(enemyType);
            enemy.SpawnOn(spawnPoint);
            _instance._enemies.Add(enemy);
        }

        public static Shell SpawnShell()
        {
            var shell = SceneContext.I.WarFactory.Shell;
            _instance._nonEnemies.Add(shell);
            return shell;
        }

        public static Explosion SpawnExplosion()
        {
            var explosion = SceneContext.I.WarFactory.Explosion;
            _instance._nonEnemies.Add(explosion);
            return explosion;
        }

        public static void EnemyReachedDestination()
        {
            _instance.PlayerHealth--;
        }

        void IPauseHandler.SetPaused(bool isPaused)
        {
            _enemies.SetPaused(isPaused);
        }
    }
}
