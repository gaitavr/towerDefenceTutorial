using Gameplay;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace MainMenu
{
    public sealed class BoardsEditorMenuItem : MonoBehaviour
    {
        [SerializeField] private TMP_InputField _nameField;
        [SerializeField] private TMP_Text _sizeText;
        [SerializeField] private Button _selectButton;
        [SerializeField] private Button _deleteButton;
        [SerializeField] private Button _renameButton;
        [SerializeField] private Toggle _currentToggle;

        private IBoardEditorMenuController _controller;
        private string _initialName;

        public BoardContext Context { get; private set; }

        public void Init(BoardContext context, IBoardEditorMenuController controller)
        {
            Context = context;
            _controller = controller;

            _nameField.text = _initialName = Context.Name;
            _sizeText.text = $"Size: {Context.Size.x}x{Context.Size.y}";
            _currentToggle.isOn = context.IsSelected;

            _selectButton.onClick.AddListener(() =>
            {
                _controller.Select(this);
            });
            _deleteButton.onClick.AddListener(() =>
            {
                _controller.Delete(this);
            });
            _renameButton.onClick.AddListener(() =>
            {
                _controller.Rename(this, _nameField.text);
            });
            _currentToggle.onValueChanged.AddListener((isActive) =>
            {
                if(isActive)
                    _controller.SetActive(this);
            });
            _nameField.onValueChanged.AddListener(OnNameChanged);
            OnNameChanged(Context.Name);
        }

        public void SetInactive() => _currentToggle.isOn = false;

        private void OnNameChanged(string text)
        {
            _renameButton.gameObject.SetActive(text != _initialName);
        }
    }
}