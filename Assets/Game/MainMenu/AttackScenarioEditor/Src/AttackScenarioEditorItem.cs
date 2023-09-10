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

        public void Init(UserAttackScenarioState.Wave wave)
        {
            foreach (var sequence in wave.Sequences)
            {
                var enemyGroup = _enemiesGroups.First(g => g.EnemyType == sequence.EnemyType);
                enemyGroup.SetInitialCount(sequence.Count);
            }
        }

        [Serializable]
        private sealed class EnemyGroup
        {
            public EnemyType EnemyType;
            public TMP_InputField EnemyCountField;

            public void SetInitialCount(int count)
            {
                EnemyCountField.text = count.ToString();
            }
        }
    }
}
