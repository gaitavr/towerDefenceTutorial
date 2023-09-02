using Core;
using GamePlay.Attack;
using GamePlay.Defend;
using System;
using UnityEngine;
using static Core.UserAttackScenarioState;

namespace Gameplay
{
    public sealed class AttackScenarioProcessor
    {
        private readonly UserAttackScenarioState _scenario;
        private readonly EnemyFactory _enemyFactory;
        private readonly GameBoard _gameBoard;

        private int _currendWaveIndex;
        private int _currentSequenceIndex;
        private float _spawnCooldown;
        private int _spawnCount;

        public bool IsRunning { get; set; }
        public event Action<GameBehavior> EnemySpawned;

        public AttackScenarioProcessor(UserAttackScenarioState scenario, EnemyFactory enemyFactory,
            GameBoard gameBoard)
        {
            _scenario = scenario;
            _enemyFactory = enemyFactory;
            _gameBoard = gameBoard;
        }

        public (int currentWave, int wavesCount) GetWaves()
        {
            return (_currendWaveIndex + 1, _scenario.Waves.Count + 1);
        }

        public bool Process()
        {
            if (_currendWaveIndex >= _scenario.Waves.Count)
                return false;

            var wave = _scenario.Waves[_currendWaveIndex];
            var isWaveFinished = ProcessWave(wave);
            if (isWaveFinished)
                _currendWaveIndex++;

            return _currendWaveIndex < _scenario.Waves.Count;
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
                SpawnEnemy(_enemyFactory, sequence.EnemyType);
            }

            return _spawnCount >= sequence.Count;
        }

        private void SpawnEnemy(EnemyFactory factory, EnemyType enemyType)
        {
            var spawnPoint = _gameBoard.GetRandomSpawnPoint();
            var enemy = factory.Get(enemyType);
            enemy.SpawnOn(spawnPoint);
            EnemySpawned?.Invoke(enemy);
        }
    }
}
