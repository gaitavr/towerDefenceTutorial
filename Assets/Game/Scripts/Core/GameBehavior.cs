using UnityEngine;

public abstract class GameBehavior : MonoBehaviour
{
    public virtual bool GameUpdate() => true;

    public abstract void Recycle();
    
    public virtual void SetPaused(bool isPaused){}
}
