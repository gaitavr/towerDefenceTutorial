using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class EnemyCollection
{
    private List<Enemy> _enemies = new List<Enemy>();
    private List<Transform> _transforms = new List<Transform>();

    public void Add(Enemy enemy)
    {
        _enemies.Add(enemy);
        _transforms.Add(enemy.transform);
    }

    public void GameUpdate()
    {
        for (int i = 0; i < _enemies.Count; i++)
        {
            if (!_enemies[i].GameUpdate())
            {
                int lastIndex = _enemies.Count - 1;
                _enemies[i] = _enemies[lastIndex];
                _enemies.RemoveAt(lastIndex);
                i -= 1;
            }
        }
    }

    public Transform[] GetAllTransforms() => _transforms.ToArray();
}
