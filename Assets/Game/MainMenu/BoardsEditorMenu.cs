using System.Collections.Generic;
using Core.Loading;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Cysharp.Threading.Tasks;
using Core;
using Utils.Serialization;
using Utils;

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
            LoadBoard(new BoardContext(_boardNameField.text, boardSize));
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
            var loadingOperations = new Queue<ILoadingOperation>();
            loadingOperations.Enqueue(new EditorModeLoadingOperation(boardContext));
            ProjectContext.I.LoadingScreenProvider.LoadAndDestroy(loadingOperations).Forget();
        }

        public void Select(BoardsEditorMenuItem item) => LoadBoard(item.Context);

        public void Delete(BoardsEditorMenuItem item)
        {
            //TODO confirm
            UserState.TryDeleteBoard(item.Context.Name);
            //TODO save 
            _items.Remove(item);
            Destroy(item.gameObject);
        }

        public void Rename(BoardsEditorMenuItem item, string newName)
        {
            //TODO confirm
            var board = UserState.TryGetBoard(item.Context.Name);
            if (board == null)
                return;
            board.Name = newName;
            //TODO save 
        }
    }
}