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
using Core;
using GamePlay.Defend;
using Gameplay;

namespace GamePlay.Modes
{
    public sealed class QuickGameMode : MonoBehaviour, IGameModeCleaner, IPauseHandler, IGameEntityInteructionProxy
    {
        [SerializeField] private Vector2Int _boardSize;
        [SerializeField] private DefenderHud _defenderHud;
        [SerializeField] private GameResultWindow _gameResultWindow;
        [SerializeField] private PrepareGamePanel _prepareGamePanel;
        [Space]
        [SerializeField, Range(0, 100)] private int _startingPlayerHealth = 10;
        [SerializeField, Range(5f, 30f)] private float _prepareTime = 15f;

        private AttackScenarioProcessor _attackScenarioExecutor;
        private CancellationTokenSource _prepareCancellation;
        private SceneInstance _environment;
        private int _playerHealth;
        private bool _isInited;

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
        private UserAccountState UserState => ProjectContext.I.UserContainer.State;

        public void Init(SceneInstance environment)
        {
            ProjectContext.I.PauseManager.Register(this);
            SceneContext.I.Initialize(this);
            _environment = environment;
            var boardData = UserBoardState.GetInitial(_boardSize, $"quick_{_boardSize}");
            GameBoard.Initialize(boardData);
            _attackScenarioExecutor = new AttackScenarioProcessor(UserState.AttackScenario, SceneContext.I.EnemyFactory, GameBoard);
            _attackScenarioExecutor.EnemySpawned += OnEnemySpawned;
            _isInited = true;
        }

        private void Update()
        {
            if (IsPaused || _isInited == false)
                return;

            UpdateScenario();
            _enemies.GameUpdate();
            Physics.SyncTransforms();
            GameBoard.GameUpdate();
            SceneContext.I.GameTileRaycaster.GameUpdate();
            TilesBuilder.GameUpdate();
            _nonEnemies.GameUpdate();
        }

        private void UpdateScenario()
        {
            if (_attackScenarioExecutor != null && _attackScenarioExecutor.IsRunning)
            {
                var waves = _attackScenarioExecutor.GetWaves();
                _defenderHud.UpdateScenarioWaves(waves.currentWave, waves.wavesCount);

                if (PlayerHealth <= 0)
                {
                    _attackScenarioExecutor.IsRunning = false;
                    _gameResultWindow.Show(GameResultType.Defeat, Restart, GoToMainMenu);
                }

                if (_attackScenarioExecutor.Process() == false && _enemies.IsEmpty)
                {
                    _attackScenarioExecutor.IsRunning = false;
                    _gameResultWindow.Show(GameResultType.Victory, Restart, GoToMainMenu);
                }
            }
        }

        public async void BeginNewGame()
        {
            ProjectContext.I.UserContainer.IsFreeTiles = true;
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
                    _attackScenarioExecutor.IsRunning = true;
                }
            }
            catch (TaskCanceledException _)
            {

            }
        }

        public void Restart()
        {
            Cleanup();
            BeginNewGame();
        }

        public void Cleanup()
        {
            _defenderHud.QuitGame -= GoToMainMenu;
            TilesBuilder.SetActive(false);
            if(_attackScenarioExecutor != null)
                _attackScenarioExecutor.EnemySpawned -= OnEnemySpawned;
            _attackScenarioExecutor = null;
            _prepareCancellation?.Cancel();
            _prepareCancellation?.Dispose();
            _enemies.Clear();
            _nonEnemies.Clear();
            GameBoard.Clear();
            ProjectContext.I.UserContainer.IsFreeTiles = false;
        }

        private void GoToMainMenu()
        {
            ProjectContext.I.AssetProvider.UnloadAdditiveScene(_environment).Forget();
            ProjectContext.I.LoadingScreenProvider.LoadAndDestroy(new ClearGameOperation(this))
                .Forget();
        }

        Shell IGameEntityInteructionProxy.SpawnShell()
        {
            var shell = SceneContext.I.WarFactory.Shell;
            _nonEnemies.Add(shell);
            return shell;
        }

        Explosion IGameEntityInteructionProxy.SpawnExplosion()
        {
            var explosion = SceneContext.I.WarFactory.Explosion;
            _nonEnemies.Add(explosion);
            return explosion;
        }

        void IGameEntityInteructionProxy.EnemyReachedDestination(int damage)
        {
            PlayerHealth -= damage;
        }

        private void OnEnemySpawned(GameBehavior gameBehavior)
        {
            _enemies.Add(gameBehavior);
        }

        void IPauseHandler.SetPaused(bool isPaused)
        {
            _enemies.SetPaused(isPaused);
        }
    }
}
