using Defend.Debuffs;
using UnityEngine;

public class DebuffContentTile : GameTileContent
{
    [SerializeField] 
    private TargetPointTrigger _trigger;

    private IDebuff _currentDebuff;

    private void Awake()
    {
        _trigger.Entered += OnTargetEntered;
    }

    private void OnTargetEntered(TargetPoint targetPoint)
    {
        _currentDebuff = Type.GetDebuff(OriginFactory, Level);
        targetPoint.Enemy.DebuffWrapper.Replace(_currentDebuff);
    }

    public override void GameUpdate()
    {
        base.GameUpdate();
        _trigger.UpdateSelf();
    }

    private void OnDestroy()
    {
        _trigger.Entered -= OnTargetEntered;
        _currentDebuff?.Remove();
    }
}