using System;
using UnityEngine;

[CreateAssetMenu]
public class GameScenario : ScriptableObject
{
    [SerializeField]
    private EnemyWave[] _waves;

    public State Begin() => new State(this);

    [Serializable]
    public struct State
    {
        private GameScenario _scenario;
        private int _index;
        private EnemyWave.State _wave;

        public (int currentWave, int wavesCount) GetWaves()
        {
            return (_index + 1, _scenario._waves.Length + 1);
        }

        public State(GameScenario scenario)
        {
            _scenario = scenario;
            _index = 0;
            _wave = scenario._waves[0].Begin();
        }

        public bool Progress()
        {
            float deltaTime = _wave.Progress(Time.deltaTime);
            while (deltaTime >= 0f)
            {
                if (++_index >= _scenario._waves.Length)
                {
                    return false;
                }
                _wave = _scenario._waves[_index].Begin();
                deltaTime = _wave.Progress(deltaTime);
            }
            return true;
        }
    }
}