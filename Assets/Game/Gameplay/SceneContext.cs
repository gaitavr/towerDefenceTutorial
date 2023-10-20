using GamePlay;
using UnityEngine;
using System.Collections.Generic;
using GamePlay.Defend;
using GamePlay.Attack;
using GamePlay.Modes;

namespace Core
{
    public class SceneContext : MonoBehaviour
    {
        [SerializeField] private GameTileContentFactory _contentFactory;
        [SerializeField] private WarFactory _warFactory;
        [SerializeField] private EnemyFactory _enemyFactory;
        [SerializeField] private Camera _mainCamera;
        [SerializeField] private GameBoard _board;
        [SerializeField] private GamePlayUI _gamePlayUI;

        private readonly List<GameTileViewController> _tileViewControllers =
            new List<GameTileViewController>();

        public TilesBuilderViewController TilesBuilder { get; private set; }
        public GameTileContentFactory ContentFactory => _contentFactory;
        public WarFactory WarFactory => _warFactory;
        public EnemyFactory EnemyFactory => _enemyFactory;
        public GameBoard GameBoard => _board;
        public GameTileRaycaster GameTileRaycaster { get; private set; }
        public TilesViewControllerRouter TilesViewControllerRouter { get; private set; }
        public IEnumerable<GameTileViewController> TileViewControllers => _tileViewControllers;
        public IGameEntityInteructionProxy EnemyInteructionProxy { get; private set; }

        public static SceneContext I { get; private set; }

        private void Awake()
        {
            I = this;
        }
        
        public void Initialize(IGameEntityInteructionProxy enemyInteructionProxy)
        {
            EnemyInteructionProxy = enemyInteructionProxy;

            GameTileRaycaster = new GameTileRaycaster(_mainCamera, GameBoard);
            TilesViewControllerRouter = new TilesViewControllerRouter(GameTileRaycaster);
            TilesBuilder = new TilesBuilderViewController(ContentFactory, GameTileRaycaster,
                GameBoard, _gamePlayUI, TilesViewControllerRouter);
            _tileViewControllers.Add(TilesBuilder);
            var wallVC = new ModifyTileViewController(GameTileContentType.Wall, ContentFactory,
                GameBoard, _gamePlayUI, TilesViewControllerRouter);
            _tileViewControllers.Add(wallVC);
            var laserVC = new ModifyTileViewController(GameTileContentType.LaserTower, ContentFactory,
                GameBoard, _gamePlayUI, TilesViewControllerRouter);
            _tileViewControllers.Add(laserVC);
            var mortarVC = new ModifyTileViewController(GameTileContentType.MortarTower, ContentFactory,
                GameBoard, _gamePlayUI, TilesViewControllerRouter);
            _tileViewControllers.Add(mortarVC);
            var iceVC = new ModifyTileViewController(GameTileContentType.Ice, ContentFactory,
                GameBoard, _gamePlayUI, TilesViewControllerRouter);
            _tileViewControllers.Add(iceVC);
            var lavaVC = new ModifyTileViewController(GameTileContentType.Lava, ContentFactory,
                GameBoard, _gamePlayUI, TilesViewControllerRouter);
            _tileViewControllers.Add(lavaVC);
            var destinationVC = new ModifyTileViewController(GameTileContentType.Destination, ContentFactory,
                GameBoard, _gamePlayUI, TilesViewControllerRouter);
            _tileViewControllers.Add(destinationVC);
            var spawnPointVC = new ModifyTileViewController(GameTileContentType.SpawnPoint, ContentFactory,
                GameBoard, _gamePlayUI, TilesViewControllerRouter);
            _tileViewControllers.Add(spawnPointVC);
        }
    }
}