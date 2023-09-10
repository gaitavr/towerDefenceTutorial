using Core;
using GamePlay.Attack;
using System;
using System.Linq;
using TMPro;
using UnityEngine;

namespace MainMenu
{
    public sealed class AttackScenarioEditorItem : MonoBehaviour
    {
        [SerializeField] private EnemyGroup[] _enemiesGroups;

        private IAttackScenarioViewController _viewController;
        
        private void Awake()
        {
            foreach (var enemyGroup in _enemiesGroups)
            {
                enemyGroup.OnCountChanged(OnEnemyCountChanged);
            }
        }

        private void OnDestroy()
        {
            foreach (var enemyGroup in _enemiesGroups)
            {
                enemyGroup.Dispose();
            }
        }

        public int GetEnemiesCount(EnemyType enemyType)
        {
            var enemyGroup = _enemiesGroups.First(g => g.EnemyType == enemyType);
            return enemyGroup.CurrentCount;
        }

        public void Init(UserAttackScenarioState.Wave wave, IAttackScenarioViewController viewController)
        {
            _viewController = viewController;
            foreach (var sequence in wave.Sequences)
            {
                var enemyGroup = _enemiesGroups.First(g => g.EnemyType == sequence.EnemyType);
                enemyGroup.EnemyCountField.text = sequence.Count.ToString();
                if(enemyGroup.InitialCount < 0)
                    enemyGroup.InitialCount = sequence.Count;

            }
        }

        private void OnEnemyCountChanged(EnemyGroup enemyGroup)
        {
            var difference = enemyGroup.CurrentCount - enemyGroup.InitialCount;
            _viewController.SetNewEnemyCount(enemyGroup.EnemyType, difference);
        }

        [Serializable]
        private sealed class EnemyGroup
        {
            public EnemyType EnemyType;
            public TMP_InputField EnemyCountField;

            public int InitialCount { get; set; } = -1;
            public int CurrentCount => int.Parse(EnemyCountField.text);

            public void OnCountChanged(Action<EnemyGroup> action)
            {
                EnemyCountField.onValueChanged.AddListener((t) =>
                {
                    if(CurrentCount >= InitialCount)
                        action(this);
                });
            }

            public void Dispose()
            {
                EnemyCountField.onValueChanged.RemoveAllListeners();
            }
        }
    }
}
