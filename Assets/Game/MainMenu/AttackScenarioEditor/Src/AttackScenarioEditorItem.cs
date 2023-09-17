using Core;
using GamePlay.Attack;
using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

namespace MainMenu
{
    public sealed class AttackScenarioEditorItem : MonoBehaviour
    {
        [SerializeField] private EnemyGroup[] _enemiesGroups;

        private IAttackScenarioViewController _viewController;

        public int GetEnemiesCount(EnemyType enemyType)
        {
            var enemyGroup = _enemiesGroups.First(g => g.EnemyType == enemyType);
            return enemyGroup.CurrentCount;
        }

        public void Init(UserAttackScenarioState.Wave wave, IAttackScenarioViewController viewController)
        {
            foreach (var enemyGroup in _enemiesGroups)
            {
                enemyGroup.EnemyCountField.onValueChanged.RemoveAllListeners();
            }
            
            _viewController = viewController;
            foreach (var sequence in wave.Sequences)
            {
                var enemyGroup = _enemiesGroups.First(g => g.EnemyType == sequence.EnemyType);
                enemyGroup.EnemyCountField.text = sequence.Count.ToString();
                if(enemyGroup.InitialCount < 0)
                    enemyGroup.InitialCount = sequence.Count;
            }
            
            foreach (var enemyGroup in _enemiesGroups)
            {
                enemyGroup.EnemyCountField.onValueChanged.AddListener(OnEnemyCountChanged);
            }
        }
        
        public IReadOnlyDictionary<EnemyType, int> GetDifference()
        {
            var result = new Dictionary<EnemyType, int>();
            foreach (var group in _enemiesGroups)
            {
                result[group.EnemyType] = group.CurrentCount - group.InitialCount;
            }
            return result;
        }

        private void OnEnemyCountChanged(string _)
        {
            _viewController.Recalculate();
        }

        [Serializable]
        private sealed class EnemyGroup
        {
            public EnemyType EnemyType;
            public TMP_InputField EnemyCountField;

            public int InitialCount { get; set; } = -1;
            public int CurrentCount => int.TryParse(EnemyCountField.text, out var count) ? count : 0;
        }
    }
}
