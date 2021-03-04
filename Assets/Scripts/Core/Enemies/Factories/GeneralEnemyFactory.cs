using UnityEngine;

[CreateAssetMenu(fileName = "General Enemy Factory", menuName = "Factories/Enemy/General")]
public class GeneralEnemyFactory : EnemyFactory
{
    [SerializeField] private EnemyConfig _chomper, _elien, _golem, _grenadier;
    
    protected override EnemyConfig GetConfig(EnemyType type)
    {
        switch (type)
        {
            case EnemyType.Golem:
                return _golem;
            case EnemyType.Elien:
                return _elien;
            case EnemyType.Chomper:
                return _chomper;
            case EnemyType.Grenadier:
                return _grenadier;
        }
        Debug.LogError($"No config for: {type}");
        return _elien;
    }
}