using System.Collections.Generic;

public interface ICleanUp
{
    IEnumerable<GameObjectFactory> Factories { get; }
    string SceneName { get; }
    void Cleanup();
}