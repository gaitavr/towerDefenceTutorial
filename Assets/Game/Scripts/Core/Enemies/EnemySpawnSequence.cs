using System;
using UnityEngine;

[Serializable]
public class EnemySpawnSequence
{
    [SerializeField]
    private EnemyFactory _factory;

    [SerializeField]
    private EnemyType _type = EnemyType.Elien;

    [SerializeField, Range(1, 100)]
    private int _amount = 1;

    [SerializeField, Range(0.1f, 10f)]
    private float _cooldown = 1f;

    public State Begin() => new State(this);

    [Serializable]
    public struct State
    {
        private EnemySpawnSequence _sequence;
        private int _count;
        private float _cooldown;

        public State(EnemySpawnSequence sequence)
        {
            _sequence = sequence;
            _count = 0;
            _cooldown = sequence._cooldown;
        }

        public float Progress(float deltaTime)
        {
            _cooldown += deltaTime;
            while (_cooldown >= _sequence._cooldown)
            {
                _cooldown -= _sequence._cooldown;
                if (_count >= _sequence._amount)
                {
                    return _cooldown;
                }
                _count += 1;
                QuickGame.SpawnEnemy(_sequence._factory, _sequence._type);
            }
            return -1f;
        }
    }
}
