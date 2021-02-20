using System;
using UnityEngine;
using System.Collections.Generic;

public class IceObstacle : GameTileContent
{
    [SerializeField]
    private IceTrigger _iceTrigger;

    private static readonly Dictionary<TargetPoint, Guid> _globalTargetStorage = 
        new Dictionary<TargetPoint, Guid>();
    private readonly Dictionary<TargetPoint, Guid> _internalTargetStorage = 
        new Dictionary<TargetPoint, Guid>();
    
    private void Awake()
    {
        _iceTrigger.Entered += OnTargetEntered;
        _iceTrigger.Exited += OnTargetExited;
    }

    private void OnTargetEntered(TargetPoint targetPoint)
    {
        var guid = Guid.NewGuid();
        _globalTargetStorage[targetPoint] = guid;
        _internalTargetStorage[targetPoint] = guid;
        targetPoint.Enemy.SetSpeed(0.5f);
    }
    
    private void OnTargetExited(TargetPoint targetPoint)
    {
        var guidGlobal = _globalTargetStorage[targetPoint];
        var guidInternal = _internalTargetStorage[targetPoint];
        _internalTargetStorage.Remove(targetPoint);
        if(guidGlobal != guidInternal)
            return;
        targetPoint.Enemy.SetSpeed(1);
        _globalTargetStorage.Remove(targetPoint);
    }
    
    private void OnDestroy()
    {
        _iceTrigger.Entered -= OnTargetEntered;
        _iceTrigger.Exited -= OnTargetExited;
    }
}