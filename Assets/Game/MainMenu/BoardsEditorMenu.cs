using System.Collections.Generic;
using Core.Loading;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Cysharp.Threading.Tasks;
using Core;
using Utils;
using System;
using Gameplay;
using Utils.Extensions;

namespace MainMenu
{
    public sealed class BoardsEditorMenu : MonoBehaviour, IBoardEditorMenuController
    {
        [SerializeField] private Canvas _canvas;
        [SerializeField] private Transform _contentParent;
        [SerializeField] private Button _closeButton;
        [SerializeField] private Button _createNewButton;
        [SerializeField] private TMP_InputField _boardNameField;
        [SerializeField] private Slider _boardSizeXslider;
        [SerializeField] private Slider _boardSizeYslider;
        [SerializeField] private BoardsEditorMenuItem _itemPrefab;

        private readonly List<BoardsEditorMenuItem> _items = new List<BoardsEditorMenuItem>();
        private UserAccountState UserState => ProjectContext.I.UserContainer.State;

        private void Awake()
        {
            _createNewButton.onClick.AddListener(OnCreateNewClicked);
            _closeButton.onClick.AddListener(OnCloseBtnClicked);
            _boardNameField.onValueChanged.AddListener(OnBoardNameChanged);
        }

        public void Show()
        {
            _canvas.worldCamera = ProjectContext.I.UICamera;
            foreach (var board in UserState.Boards)
            {
                var item = Instantiate(_itemPrefab, _contentParent);
                item.gameObject.SetActive(true);
                _items.Add(item);
                item.Init(new BoardContext(board), this);
            }
            _canvas.enabled = true;
        }

        private void OnCreateNewClicked()
        {
            var boardSizeX = Constants.Game.MIN_BOARD_SIZE + 
                (Constants.Game.MAX_BOARD_SIZE - Constants.Game.MIN_BOARD_SIZE) * _boardSizeXslider.value;
            var boardSizeY = Constants.Game.MIN_BOARD_SIZE +
                (Constants.Game.MAX_BOARD_SIZE - Constants.Game.MIN_BOARD_SIZE) * _boardSizeYslider.value;
            var boardSize = new Vector2Int((int)boardSizeX, (int)boardSizeY);
            var isSelected = UserState.Boards.Count == 0;
            LoadBoard(new BoardContext(_boardNameField.text, boardSize, isSelected));
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

        private void OnBoardNameChanged(string text)
        {
            _createNewButton.interactable = text.Length >= Constants.Game.MIN_BOARD_NAME_LENGHT;
        }

        private void LoadBoard(BoardContext boardContext)
        {
            ProjectContext.I.LoadingScreenProvider.LoadAndDestroy(new EditorModeLoadingOperation(boardContext))
                .Forget();
        }

        public void Select(BoardsEditorMenuItem item) => LoadBoard(item.Context);

        public async void Delete(BoardsEditorMenuItem item)
        {
            try
            {
                var alertPopup = await AlertPopup.Load();
                var isConfirmed = await alertPopup.Value.AwaitForDecision("Are you sure to delete this board?");
                alertPopup.Dispose();
                if (isConfirmed == false)
                    return;

                UserState.TryDeleteBoard(item.Context.Name);
                var saveResult = await ProjectContext.I.UserStateCommunicator.SaveUserState(UserState);
                if (saveResult)
                {
                    _items.Remove(item);
                    Destroy(item.gameObject);
                }
            }
            catch (Exception e)
            {
                Debug.LogError($"Delete board exception: {e.Message}");
            }
        }

        public async void Rename(BoardsEditorMenuItem item, string newName)
        {
            try
            {
                var alertPopup = await AlertPopup.Load();
                var isConfirmed = await alertPopup.Value.AwaitForDecision("Are you sure to rename this board?");
                alertPopup.Dispose();
                if (isConfirmed == false)
                    return;

                var board = UserState.TryGetBoard(item.Context.Name);
                board.Name = newName;

                var saveResult = await ProjectContext.I.UserStateCommunicator.SaveUserState(UserState);
            }
            catch (Exception e)
            {
                Debug.LogError($"Delete board exception: {e.Message}");
            }
        }

        public async void SetActive(BoardsEditorMenuItem item)
        {
            try
            {
                foreach (var i in _items)
                {
                    var board = UserState.TryGetBoard(i.Context.Name);
                    if (item == i)
                    {
                        board.Selected = true;
                    }
                    else
                    {
                        i.SetInactive();
                        board.Selected = false;
                    }
                }

                var saveResult = await ProjectContext.I.UserStateCommunicator.SaveUserState(UserState);
            }
            catch (Exception e)
            {
                Debug.LogError($"SetActive board exception: {e.Message}");
            }
        }
    }
}