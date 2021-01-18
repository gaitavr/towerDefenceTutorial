using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[SelectionBase]
public class GameTileContent : MonoBehaviour
{
    [SerializeField]
    private GameTileContentType _type;

    public GameTileContentType Type => _type;

    public GameTileContentFactory OriginFactory { get; set; }

    public bool IsBlockingPath => Type > GameTileContentType.BeforeBlockers;

    public void Recycle()
    {
        OriginFactory.Reclaim(this);
    }

    public virtual void GameUpdate()
    {

    }
}

public enum GameTileContentType
{
    Empty = 0,
    Destination = 1,
    SpawnPoint = 2,
    
    BeforeBlockers = 50,
    Wall = 51,
    
    BeforeAttackers = 100,
    LaserTower = 101,
    MortarTower = 102
}
