using UnityEngine;

public class IceObstacle : GameTileContent
{
    [SerializeField]
    private IceTrigger _iceTrigger;

    private void Awake()
    {
        _iceTrigger.Entered += OnTargetEntered;
        _iceTrigger.Exited += OnTargetExited;
    }

    private void OnTargetEntered(TargetPoint targetPoint)
    {
        targetPoint.Enemy.SetSpeed(0.5f);
    }
    
    private void OnTargetExited(TargetPoint targetPoint)
    {
        targetPoint.Enemy.SetSpeed(1);
    }
}