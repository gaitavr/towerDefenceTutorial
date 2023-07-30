using Game.Core.GamePlay;
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
        
        public TilesBuilderViewController TilesBuilder { get; private set;}
        public GameTileContentFactory ContentFactory => _contentFactory;
        public WarFactory WarFactory => _warFactory;
        public EnemyFactory EnemyFactory => _enemyFactory;
        public Camera MainCamera => _mainCamera;
        public GameBoard GameBoard => _board;
        public GamePlayUI GamePlayUI => _gamePlayUI;
        
        public static SceneContext I { get; private set; }

        private void Awake()
        {
            I = this;
        }
        
        public void Initialize()
        {
            TilesBuilder = new TilesBuilderViewController(ContentFactory, MainCamera,
                GameBoard, true);
        }
    }
}