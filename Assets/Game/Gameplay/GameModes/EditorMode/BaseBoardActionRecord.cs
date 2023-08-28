using Core;
using GamePlay.Defend;

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
