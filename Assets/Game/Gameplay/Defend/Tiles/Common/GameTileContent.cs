using UnityEngine;

namespace GamePlay
{
    [SelectionBase]
    public class GameTileContent : MonoBehaviour
    {
        [SerializeField] private GameTileContentType _type;

        public GameTileContentType Type => _type;

        protected GameTileContentFactory OriginFactory { get; private set; }

        public int Level { get; private set; }

        public bool IsBlockingPath => Type > GameTileContentType.BeforeBlockers;

        public virtual void Initialize(GameTileContentFactory factory, int level)
        {
            OriginFactory = factory;
            Level = level;
        }

        public void ChangeType(GameTileContentType type)
        {
            _type = type;
        }

        public void Recycle()
        {
            OriginFactory.Reclaim(this);
        }

        public virtual void GameUpdate()
        {

        }
    }
}
