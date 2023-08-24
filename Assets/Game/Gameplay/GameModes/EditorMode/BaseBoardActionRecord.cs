using Core;
using Game.Defend.Tiles;

namespace GamePlay.Modes
{
    public abstract class BaseBoardActionRecord
    {
        protected GameBoard GameBoard => SceneContext.I.GameBoard;
        protected UserContainer UserContainer => ProjectContext.I.UserContainer;
        protected TilesViewControllerRouter ViewControllerRouter => SceneContext.I.TilesViewControllerRouter;

        public abstract void Undo();
    }
}
