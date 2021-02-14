using UnityEngine;

public abstract class EnemyView : MonoBehaviour
{
    public bool IsInited { get; protected set; }

    protected Animator _animator;
    protected Enemy _enemy;

    protected const string DIED_KEY = "Died";

    public virtual void Init(Enemy enemy)
    {
        _animator = GetComponent<Animator>();
        _enemy = enemy;
    }

    public virtual void Die()
    {
        _animator.SetBool(DIED_KEY, true);
    }

    public void SetSpeedFactor(float speedFactor)
    {
        _animator.speed = speedFactor;
    }

    public void OnSpawnAnimationFinished()
    {
        IsInited = true;
        GetComponent<TargetPoint>().IsEnabled = true;
    }
}
