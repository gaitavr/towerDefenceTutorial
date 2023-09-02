using Core;
using GamePlay.Modes;

namespace GamePlay.Defend
{
    public abstract class WarEntity : GameBehavior
    {
        public WarFactory OriginFactory { get; set; }

        protected IEnemyInteructionProxy InterructionProxy => SceneContext.I.EnemyInteructionProxy;

        public override void Recycle()
        {
            OriginFactory.Reclaim(this);
        }
    }
}
