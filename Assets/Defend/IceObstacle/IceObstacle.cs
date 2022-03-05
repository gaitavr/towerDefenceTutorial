using System;
using UnityEngine;
using System.Collections.Generic;

public class IceObstacle : GameTileContent
{
    [SerializeField]
    private TargetPointTrigger _trigger;
    
    private void Awake()
    {
        _trigger.Entered += OnTargetEntered;
        _trigger.Exited += OnTargetExited;
    }

    private void OnTargetEntered(TargetPoint targetPoint)
    {
        var iceSlower = targetPoint.gameObject.AddComponent<IceSlower>();
        iceSlower.Assign(targetPoint.Enemy);
    }
    
    private void OnTargetExited(TargetPoint targetPoint)
    {
        if (targetPoint.gameObject.TryGetComponent<IceSlower>(out var slower))
        {
            slower.Delete(targetPoint.Enemy);
            Destroy(slower);
        }
    }
    
    private void OnDestroy()
    {
        _trigger.Entered -= OnTargetEntered;
        _trigger.Exited -= OnTargetExited;
    }
}