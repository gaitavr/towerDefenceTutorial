using Core;
using Core.Communication;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace MainMenu
{
    public sealed class AttackScenarioEditorMenu : MonoBehaviour
    {
        [SerializeField] private Canvas _canvas;
        [SerializeField] private Transform _contentParent;
        [SerializeField] private Button _closeButton;
        [SerializeField] private Button _saveButton;
        [SerializeField] private Button _resetButton;
        [SerializeField] private TMP_Text _energyAmountText;
        [SerializeField] private AttackScenarioEditorItem _itemPrefab;

        private readonly List<AttackScenarioEditorItem> _items = new List<AttackScenarioEditorItem>();
        private UserAccountState AccountState
        {
            get => ProjectContext.I.UserContainer.State;
            set => ProjectContext.I.UserContainer.State = value;
        }

        private IUserStateCommunicator UserStateCommunicator => ProjectContext.I.UserStateCommunicator;

        private void Awake()
        {
            _saveButton.onClick.AddListener(OnSaveButtonClicked);
            _closeButton.onClick.AddListener(OnCloseBtnClicked);
            _resetButton.onClick.AddListener(OnResetButtonClicked);
        }

        public void Show()
        {
            _canvas.worldCamera = ProjectContext.I.UICamera;
            _energyAmountText.text = AccountState.Currencies.Energy.ToString();
            foreach (var wave in AccountState.AttackScenario.Waves)
            {
                var item = Instantiate(_itemPrefab, _contentParent);
                item.gameObject.SetActive(true);
                _items.Add(item);
                item.Init(wave);
            }
            _canvas.enabled = true;
        }

        private void OnSaveButtonClicked()
        {
            AccountState.AttackScenario = ConvertToState();
            UserStateCommunicator.SaveUserState(AccountState);
        }

        private UserAttackScenarioState ConvertToState()
        {
            return new UserAttackScenarioState();
        }

        private void OnResetButtonClicked()
        {
            AccountState.AttackScenario = UserAttackScenarioState.GetInitial();
            UserStateCommunicator.SaveUserState(AccountState);
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
    }
}
