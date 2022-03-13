﻿using System;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;

public class LavaObstacle : IDebuff
{
    private const int DAMAGE = 1;

    private bool _isActive;
    private Enemy _enemy;

    private bool IsNotPaused => ProjectContext.Instance.PauseManager.IsPaused;
    
    public void Assign(Enemy enemy)
    {
        _enemy = enemy;
        _isActive = true;
        DamageTask();
    }

    public void Delete(Enemy enemy)
    {
        _isActive = false;
    }

    private async UniTask DamageTask()
    {
        while (_isActive)
        {
            if(IsNotPaused)
                _enemy.TakeDamage(DAMAGE);
            await Task.Delay(TimeSpan.FromSeconds(0.1f));
        }
    }
}