﻿using System;

public class DebuffEnemyWrapper : IDisposable
{
    private readonly Enemy _enemy;
    private IDebuff _currentDebuff;

    public DebuffEnemyWrapper(Enemy enemy)
    {
        _enemy = enemy;
    }

    public void Replace(IDebuff debuff)
    {
        _currentDebuff?.Delete(_enemy);
        _currentDebuff = debuff;
        _currentDebuff.Assign(_enemy);
    }

    public void Dispose()
    {
        _currentDebuff?.Delete(_enemy);
    }
}