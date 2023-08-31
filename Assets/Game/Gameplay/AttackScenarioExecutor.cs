using Core;
using GamePlay.Attack;
using GamePlay.Modes;
using System.Collections;
using UnityEngine;
using static Core.UserAttackScenarioState;

namespace Gameplay
{
    public sealed class AttackScenarioExecutor : IEnumerator
    {
        private readonly UserAttackScenarioState _scenario;
        private readonly EnemyFactory _enemyFactory;

        private int _currendWaveIndex;
        private int _currentSequenceIndex;
        private float _spawnCooldown;
        private int _spawnCount;

        public object Current => null;

        public AttackScenarioExecutor(UserAttackScenarioState scenario, EnemyFactory enemyFactory)
        {
            _scenario = scenario;
            _enemyFactory = enemyFactory;
            _currentSequenceIndex = 3;
        }

        public (int currentWave, int wavesCount) GetWaves()
        {
            return (_currendWaveIndex + 1, _scenario.Waves.Count + 1);
        }

        public bool MoveNext()
        {
            if (_currendWaveIndex >= _scenario.Waves.Count)
                return false;

            var wave = _scenario.Waves[_currendWaveIndex];
            var isWaveFinished = ProcessWave(wave);
            if (isWaveFinished)
                _currendWaveIndex++;

            return _currendWaveIndex < _scenario.Waves.Count;
        }

        public void Reset()
        {
            _currendWaveIndex = 0;
            _currentSequenceIndex = 0;
            _spawnCooldown = 0;
            _spawnCount = 0;
        }

        private bool ProcessWave(Wave wave)
        {
            var sequence = wave.Sequences[_currentSequenceIndex];
            var isSequenceFinished = ProcessSequence(sequence);
            if (isSequenceFinished)
            {
                _currentSequenceIndex++;
                _spawnCount = 0;
            }

            return _currentSequenceIndex >= wave.Sequences.Count;
        }

        public bool ProcessSequence(SpawnSequence sequence)
        {
            _spawnCooldown += Time.deltaTime;
            if (_spawnCooldown >= sequence.Cooldown)
            {
                _spawnCooldown = 0;
                _spawnCount++;
                QuickGameMode.SpawnEnemy(_enemyFactory, sequence.EnemyType);
            }

            return _spawnCount >= sequence.Count;
        }
    }
}
