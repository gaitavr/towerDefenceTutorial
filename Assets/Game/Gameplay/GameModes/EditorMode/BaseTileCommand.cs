using Core;

namespace GamePlay.Modes
{
    public abstract class BaseTileCommand
    {
        protected GameBoard GameBoard => SceneContext.I.GameBoard;
        protected UserContainer UserContainer => ProjectContext.I.UserContainer;

        public abstract void Undo();
    }
}
