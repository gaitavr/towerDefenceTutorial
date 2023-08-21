using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class GameBehaviorCollection
{
    private List<GameBehavior> _behaviors = new();
    private List<Transform> _transforms = new();

    public bool IsEmpty => _behaviors.Count == 0;

    public void Add(GameBehavior behavior)
    {
        _behaviors.Add(behavior);
        _transforms.Add(behavior.transform);
    }

    public void GameUpdate()
    {
        for (var i = 0; i < _behaviors.Count; i++)
        {
            if (_behaviors[i].GameUpdate() == false)
            {
                var lastIndex = _behaviors.Count - 1;
                _behaviors[i] = _behaviors[lastIndex];
                _behaviors.RemoveAt(lastIndex);
                i -= 1;
            }
        }
    }

    public void Clear()
    {
        for (var i = 0; i < _behaviors.Count; i++)
        {
            _behaviors[i].Recycle();
        }
        _behaviors.Clear();
    }

    public void SetPaused(bool isPaused)
    {
        for (var i = 0; i < _behaviors.Count; i++)
        {
            _behaviors[i].SetPaused(isPaused);
        }
    }
    
    public Transform[] GetAllTransforms() => _transforms.ToArray();
}
