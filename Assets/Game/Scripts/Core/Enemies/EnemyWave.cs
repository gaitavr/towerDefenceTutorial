using System;
using UnityEngine;

[CreateAssetMenu]
public class EnemyWave : ScriptableObject
{
    [SerializeField]
    private EnemySpawnSequence[] _spawnSequences;

    public State Begin() => new State(this);

    [Serializable]
    public struct State
    {
        private EnemyWave _wave;
        private int _index;
        private EnemySpawnSequence.State _sequence;

        public State(EnemyWave wave)
        {
            _wave = wave;
            _index = 0;
            _sequence = wave._spawnSequences[0].Begin();
        }

        public float Progress(float deltaTime)
        {
            deltaTime = _sequence.Progress(deltaTime);
            while (deltaTime >= 0f)
            {
                if (++_index >= _wave._spawnSequences.Length)
                {
                    return deltaTime;
                }
                _sequence = _wave._spawnSequences[_index].Begin();
                deltaTime = _sequence.Progress(deltaTime);
            }
            return -1f;
        }
    }
}
