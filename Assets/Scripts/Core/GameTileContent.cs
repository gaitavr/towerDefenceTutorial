using Defend.Debuffs;
using UnityEngine;

[SelectionBase]
public class GameTileContent : MonoBehaviour
{
    [SerializeField]
    private GameTileContentType _type;
    [SerializeField]
    private TargetPointTrigger _trigger;

    public GameTileContentType Type => _type;

    public GameTileContentFactory OriginFactory { get; set; }

    public bool IsBlockingPath => Type > GameTileContentType.BeforeBlockers;

    private void Awake()
    {
        if(_trigger != null)
            _trigger.Entered += OnTargetEntered;
    }

    private void OnTargetEntered(TargetPoint targetPoint)
    {
        targetPoint.Enemy.DebuffWrapper.Replace(Type.GetDebuff());
    }
    
    private void OnDestroy()
    {
        if(_trigger != null)
            _trigger.Entered -= OnTargetEntered;
    }
    
    public void Recycle()
    {
        OriginFactory.Reclaim(this);
    }

    public virtual void GameUpdate()
    {
        if(_trigger != null)
            _trigger.UpdateSelf();
    }
}
