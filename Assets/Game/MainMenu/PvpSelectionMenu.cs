using Core;
using Cysharp.Threading.Tasks;
using GamePlay.Modes;
using UnityEngine;
using UnityEngine.UI;

namespace MainMenu
{
    public sealed class PvpSelectionMenu : MonoBehaviour
    {
        [SerializeField] private Canvas _canvas;
        [SerializeField] private Button _attackButton;
        [SerializeField] private Button _defendButton;
        [SerializeField] private Button _closeButton;

        private UniTaskCompletionSource<PvpGroupType> _selectionCompletion;

        public UserBoardState BoardState => UserState.Boards[0];//TODO temporary
        public UserAttackScenarioState AttackScenarioState => UserState.AttackScenarios[0];//TODO temporary

        private UserAccountState UserState => ProjectContext.I.UserContainer.State;

        private void Awake()
        {
            _attackButton.onClick.AddListener(OnAttackButtonClicked);
            _defendButton.onClick.AddListener(OnDefendButtonClicked);
            _closeButton.onClick.AddListener(OnCloseButtonClicked);
        }

        public async UniTask<PvpGroupType> SelectGroup()
        {
            _canvas.worldCamera = ProjectContext.I.UICamera;
            _selectionCompletion = new UniTaskCompletionSource<PvpGroupType>();
            _canvas.enabled = true;

            var result = await _selectionCompletion.Task;
            _canvas.enabled = false;
            return result;
        }

        private void OnAttackButtonClicked()
        {
            _selectionCompletion.TrySetResult(PvpGroupType.Attack);
        }

        private void OnDefendButtonClicked()
        {
            _selectionCompletion.TrySetResult(PvpGroupType.Defend);
        }

        private void OnCloseButtonClicked()
        {
            _selectionCompletion.TrySetResult(PvpGroupType.Unknown);
        }
    }
}
