using UnityEngine;

public abstract class EnemyView : MonoBehaviour
{
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
}
