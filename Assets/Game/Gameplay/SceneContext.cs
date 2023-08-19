using Game.Core.GamePlay;
using Game.Defend.Tiles;
using UnityEngine;

namespace Game.Core
{
    public class SceneContext : MonoBehaviour
    {
        [SerializeField] private GameTileContentFactory _contentFactory;
        [SerializeField] private WarFactory _warFactory;
        [SerializeField] private EnemyFactory _enemyFactory;
        [SerializeField] private Camera _mainCamera;
        [SerializeField] private GameBoard _board;
        [SerializeField] private GamePlayUI _gamePlayUI;

        public TilesBuilderViewController TilesBuilder { get; private set; }
        public GameTileContentFactory ContentFactory => _contentFactory;
        public WarFactory WarFactory => _warFactory;
        public EnemyFactory EnemyFactory => _enemyFactory;
        public GameBoard GameBoard => _board;
        public GameTileRaycaster GameTileRaycaster { get; private set; }

        public static SceneContext I { get; private set; }

        private void Awake()
        {
            I = this;
        }
        
        public void Initialize()
        {
            GameTileRaycaster = new GameTileRaycaster(_mainCamera, GameBoard);
            var tilesViewControllerRouter = new TilesViewControllerRouter(GameTileRaycaster);
            TilesBuilder = new TilesBuilderViewController(ContentFactory, GameTileRaycaster,
                GameBoard, _gamePlayUI, tilesViewControllerRouter);

            new ModifyTileViewController(GameTileContentType.Wall, ContentFactory,
                GameBoard, _gamePlayUI, tilesViewControllerRouter);
            new ModifyTileViewController(GameTileContentType.LaserTower, ContentFactory,
                GameBoard, _gamePlayUI, tilesViewControllerRouter);
            new ModifyTileViewController(GameTileContentType.MortarTower, ContentFactory,
                GameBoard, _gamePlayUI, tilesViewControllerRouter);
            new ModifyTileViewController(GameTileContentType.Ice, ContentFactory,
                GameBoard, _gamePlayUI, tilesViewControllerRouter);
            new ModifyTileViewController(GameTileContentType.Lava, ContentFactory,
                GameBoard, _gamePlayUI, tilesViewControllerRouter);
            new ModifyTileViewController(GameTileContentType.Destination, ContentFactory,
                GameBoard, _gamePlayUI, tilesViewControllerRouter);
            new ModifyTileViewController(GameTileContentType.SpawnPoint, ContentFactory,
                GameBoard, _gamePlayUI, tilesViewControllerRouter);
        }
    }
}