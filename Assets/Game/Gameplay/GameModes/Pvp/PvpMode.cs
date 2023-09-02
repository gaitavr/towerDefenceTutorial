using Core;
using GamePlay.Defend;
using Gameplay;
using System.Collections.Generic;
using UnityEngine;
using Core.Pause;
using Core.Loading;
using Core.UI;
using Utils.Extensions;
using Cysharp.Threading.Tasks;

namespace GamePlay.Modes
{
    public sealed class PvpMode : MonoBehaviour, IGameModeCleaner, IPauseHandler, IEnemyInteructionProxy
    {
        [SerializeField] private DefenderHud _defenderHud;

        private AttackScenarioProcessor _attackScenarioExecutor;
        private UserAttackScenarioState _scenarioState;
        private int _playerHealth;
        private int _initialPlayerHealth;
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
                _defenderHud.UpdatePlayerHealth(_playerHealth, _initialPlayerHealth);
            }
        }

        public IEnumerable<GameObjectFactory> Factories => new GameObjectFactory[]
        {
            SceneContext.I.ContentFactory, SceneContext.I.WarFactory,
            SceneContext.I.EnemyFactory
        };

        public string SceneName => Utils.Constants.Scenes.PVP_MODE;
        private GameBoard GameBoard => SceneContext.I.GameBoard;
        private UserAccountState UserState => ProjectContext.I.UserContainer.State;

        public void Init(BoardContext boardContext, UserAttackScenarioState scenarioState)
        {
            _scenarioState = scenarioState;
            ProjectContext.I.PauseManager.Register(this);
            SceneContext.I.Initialize(this);

            _initialPlayerHealth = 100;

            var boardData = UserState.TryGetBoard(boardContext.Name);
            GameBoard.Initialize(boardData);
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
            if (_attackScenarioExecutor != null && _attackScenarioExecutor.IsRunning)
            {
                var waves = _attackScenarioExecutor.GetWaves();
                _defenderHud.UpdateScenarioWaves(waves.currentWave, waves.wavesCount);

                if (PlayerHealth <= 0)
                {
                    _attackScenarioExecutor.IsRunning = false;
                    //_gameResultWindow.Show(GameResultType.Defeat, BeginNewGame, GoToMainMenu);
                }

                if (_attackScenarioExecutor.Process() == false && _enemies.IsEmpty)
                {
                    _attackScenarioExecutor.IsRunning = false;
                    //_gameResultWindow.Show(GameResultType.Victory, BeginNewGame, GoToMainMenu);
                }
            }
        }

        public void BeginNewGame()
        {
            Cleanup();
            PlayerHealth = _initialPlayerHealth;
            _defenderHud.QuitGame += GoToMainMenu;

            _attackScenarioExecutor = new AttackScenarioProcessor(_scenarioState, SceneContext.I.EnemyFactory, GameBoard);
            _attackScenarioExecutor.EnemySpawned += OnEnemySpawned;
        }

        public void Cleanup()
        {
            _defenderHud.QuitGame -= GoToMainMenu;
            if (_attackScenarioExecutor != null)
                _attackScenarioExecutor.EnemySpawned -= OnEnemySpawned;
            _attackScenarioExecutor = null;
            _enemies.Clear();
            _nonEnemies.Clear();
            GameBoard.Clear();
        }

        private void GoToMainMenu()
        {
            ProjectContext.I.LoadingScreenProvider.LoadAndDestroy(new ClearGameOperation(this))
                .Forget();
        }

        Shell IEnemyInteructionProxy.SpawnShell()
        {
            var shell = SceneContext.I.WarFactory.Shell;
            _nonEnemies.Add(shell);
            return shell;
        }

        Explosion IEnemyInteructionProxy.SpawnExplosion()
        {
            var explosion = SceneContext.I.WarFactory.Explosion;
            _nonEnemies.Add(explosion);
            return explosion;
        }

        void IEnemyInteructionProxy.EnemyReachedDestination(int damage)
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
