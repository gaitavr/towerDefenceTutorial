using System.Collections.Generic;

namespace GamePlay.Modes
{
    public interface IGameModeCleaner
    {
        IEnumerable<GameObjectFactory> Factories { get; }
        string SceneName { get; }
        void Cleanup();
    }
}