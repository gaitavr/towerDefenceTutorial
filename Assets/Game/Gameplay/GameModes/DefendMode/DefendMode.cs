using System.Collections.Generic;
using System.Linq;
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
    public sealed class DefendMode : MonoBehaviour, IGameModeCleaner, IPauseHandler, IGameEntityInteructionProxy
    {
        [SerializeField] private HudUI _hudUI;
        [SerializeField] private GameResultWindow _gameResultWindow;

        private AttackScenarioProcessor _attackScenarioExecutor;
        private SceneInstance _environment;
        private int _currentPlayerHealth;
        private int _maxPlayerHealth;
        private bool _isInited;

        private readonly GameBehaviorCollection _enemies = new();
        private readonly GameBehaviorCollection _nonEnemies = new();

        private bool IsPaused => ProjectContext.I.PauseManager.IsPaused;

        private int PlayerHealth
        {
            get => _currentPlayerHealth;
            set
            {
                _currentPlayerHealth = Mathf.Max(0, value);
                _hudUI.UpdatePlayerHealth(_currentPlayerHealth, _maxPlayerHealth);
            }
        }

        public IEnumerable<GameObjectFactory> Factories => new GameObjectFactory[]
        {
            SceneContext.I.ContentFactory, SceneContext.I.WarFactory,
            SceneContext.I.EnemyFactory
        };
        public string SceneName => Utils.Constants.Scenes.DEFEND_MODE;
        private GameBoard GameBoard => SceneContext.I.GameBoard;
        private UserAccountState UserState => ProjectContext.I.UserContainer.State;

        public void Init(UserAttackScenarioState scenarioState, SceneInstance environment)
        {
            ProjectContext.I.PauseManager.Register(this);
            SceneContext.I.Initialize(this);
            
            _environment = environment;
            var selectedBoard = UserState.Boards.FirstOrDefault(b => b.Selected);
            GameBoard.Initialize(selectedBoard);
            
            _attackScenarioExecutor = new AttackScenarioProcessor(scenarioState, SceneContext.I.EnemyFactory, GameBoard);
            _attackScenarioExecutor.EnemySpawned += OnEnemySpawned;
            
            _maxPlayerHealth = 100;
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
            _nonEnemies.GameUpdate();
        }

        private void UpdateScenario()
        {
            if (_attackScenarioExecutor is not { IsRunning: true }) 
                return;
            
            var waves = _attackScenarioExecutor.GetWaves();
            _hudUI.UpdateScenarioWaves(waves.currentWave, waves.wavesCount);

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

        public void BeginNewGame()
        {
            PlayerHealth = _maxPlayerHealth;
            _hudUI.QuitGame += GoToMainMenu;
            _attackScenarioExecutor.IsRunning = true;
        }

        public void Restart()
        {
            Cleanup();
            BeginNewGame();
        }

        public void Cleanup()
        {
            _hudUI.QuitGame -= GoToMainMenu;
            if(_attackScenarioExecutor != null)
                _attackScenarioExecutor.EnemySpawned -= OnEnemySpawned;
            _attackScenarioExecutor = null;
            _enemies.Clear();
            _nonEnemies.Clear();
            GameBoard.Clear();
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
