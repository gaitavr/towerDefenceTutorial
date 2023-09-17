using System;
using Core;
using Core.Communication;
using System.Collections.Generic;
using System.Linq;
using GamePlay.Attack;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Utils;

namespace MainMenu
{
    public sealed class AttackScenarioEditorMenu : MonoBehaviour, IAttackScenarioViewController
    {
        private const string YOUR_ENERGY = "Your energy";
        private const string ENERGY_USED = "Energy used";

        [SerializeField] private Canvas _canvas;
        [SerializeField] private Transform _contentParent;
        [SerializeField] private Button _closeButton;
        [SerializeField] private Button _saveButton;
        [SerializeField] private Button _resetButton;
        [SerializeField] private TMP_Text _energyAmountText;
        [SerializeField] private TMP_Text _usedEnergyText;
        [SerializeField] private AttackScenarioEditorItem _itemPrefab;

        private readonly List<AttackScenarioEditorItem> _items = new();
        private int _usedEnergy;

        private UserAccountState AccountState => ProjectContext.I.UserContainer.State;

        private IUserStateCommunicator UserStateCommunicator => ProjectContext.I.UserStateCommunicator;
        private UserContainer UserContainer => ProjectContext.I.UserContainer;

        private void Awake()
        {
            _saveButton.onClick.AddListener(OnSaveButtonClicked);
            _closeButton.onClick.AddListener(OnCloseBtnClicked);
            _resetButton.onClick.AddListener(OnResetButtonClicked);
        }

        public void Show()
        {
            foreach (var item in AccountState.AttackScenario.Waves.Select(_ => Instantiate(_itemPrefab, _contentParent)))
            {
                item.gameObject.SetActive(true);
                _items.Add(item);
            }

            SetEnergyText(_energyAmountText, AccountState.Currencies.Energy, YOUR_ENERGY);
            ReInit();

            _canvas.worldCamera = ProjectContext.I.UICamera;
            _canvas.enabled = true;
        }

        private void ReInit()
        {
            for (var i = 0; i < _items.Count; i++)
            {
                var wave = AccountState.AttackScenario.Waves[i];
                _items[i].Init(wave, this);
            }
            
            _usedEnergy = 0;
            SetEnergyText(_usedEnergyText, _usedEnergy, ENERGY_USED);
        }

        private async void OnSaveButtonClicked()
        {
            if (_usedEnergy > AccountState.Currencies.Energy)
                return;

            var alertPopup = await AlertPopup.Load();
            var decision = await alertPopup.Value.AwaitForDecision("Are your sure to modify scenario?");
            alertPopup.Dispose();

            if (decision)
            {
                AccountState.Currencies.ChangeEnergy(-_usedEnergy);
                ModifyState();
                UserStateCommunicator.SaveUserState(AccountState);
                
                SetEnergyText(_energyAmountText, AccountState.Currencies.Energy, YOUR_ENERGY);

                ReInit();
            }
        }

        private void ModifyState()
        {
            for (var i = 0; i < _items.Count; i++)
            {
                var wave = AccountState.AttackScenario.Waves[i];
                foreach (var sequence in wave.Sequences)
                {
                    sequence.Count = _items[i].GetEnemiesCount(sequence.EnemyType);
                }
            }
        }

        private async void OnResetButtonClicked()
        {
            var alertPopup = await AlertPopup.Load();
            var decision = await alertPopup.Value.AwaitForDecision("Are your sure to reset scenario?");
            alertPopup.Dispose();
            
            if(decision == false)
                return;
            
            AccountState.AttackScenario = UserAttackScenarioState.GetInitial();
            UserStateCommunicator.SaveUserState(AccountState);
            ReInit();
        }

        private void OnCloseBtnClicked()
        {
            _canvas.enabled = false;
            foreach (var element in _items)
            {
                Destroy(element.gameObject);
            }
            _items.Clear();
        }

        private void SetEnergyText(TMP_Text textSource, int amount, string prefix)
        {
            textSource.text = $"{prefix}: {amount}";
        }

        void IAttackScenarioViewController.Recalculate()
        {
            _usedEnergy = CalculateUsedEnergy();
            SetEnergyText(_usedEnergyText, _usedEnergy, ENERGY_USED);
        }

        private int CalculateUsedEnergy()
        {
            var result = 0;
            foreach (var item in _items)
            {
                var difference = item.GetDifference();
                foreach (var diff in difference)
                {
                    var temp = UserContainer.CalculateUsedEnergy(diff.Key, diff.Value);
                    temp = Mathf.Max(0, temp);
                    result += temp;
                }
            }
            return result;
        }
    }
}
