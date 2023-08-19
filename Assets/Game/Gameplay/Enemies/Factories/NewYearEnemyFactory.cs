using UnityEngine;

[CreateAssetMenu(fileName = "New Year Enemy Factory", menuName = "Factories/Enemy/NewYear")]
public class NewYearEnemyFactory : EnemyFactory
{
    [SerializeField] private EnemyConfig _snowGolem, _santa, _snowChomper;

    protected override EnemyConfig GetConfig(EnemyType type)
    {
        switch (type)
        {
            case EnemyType.Golem:
                return _snowGolem;
            case EnemyType.Elien:
            case EnemyType.Chomper:
                return _snowChomper;
            case EnemyType.Grenadier:
                return _santa;
        }
        Debug.LogError($"No config for: {type}");
        return _santa;
    }
}