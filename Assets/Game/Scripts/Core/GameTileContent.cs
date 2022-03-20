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

    protected GameTileContentFactory OriginFactory { get; private set; }
    
    public int Level { get; private set; }

    public bool IsBlockingPath => Type > GameTileContentType.BeforeBlockers;

    private void Awake()
    {
        if(_trigger != null)
            _trigger.Entered += OnTargetEntered;
    }

    public virtual void Initialize(GameTileContentFactory factory, int level)
    {
        OriginFactory = factory;
        Level = level;
    }

    private void OnTargetEntered(TargetPoint targetPoint)
    {
        var debuff = Type.GetDebuff(OriginFactory, Level);
        targetPoint.Enemy.DebuffWrapper.Replace(debuff);
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
