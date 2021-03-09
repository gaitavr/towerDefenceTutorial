using System;
using System.Collections.Generic;
using System.Linq;
using Loading;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace MainMenu
{
    public class EditorMenu : MonoBehaviour
    {
        [SerializeField]
        private Canvas _canvas;
        [SerializeField]
        private Transform _contentParent;
        [SerializeField]
        private Button _closeButton;
        [SerializeField]
        private Button _createNewButton;
        [SerializeField]
        private TMP_InputField _newSaveField;
        [SerializeField]
        private EditorElement _elementPrefab;

        private readonly List<EditorElement> _elements = new List<EditorElement>();
        private readonly BoardSerializer _boardSerializer = new BoardSerializer();

        private void Awake()
        {
            _closeButton.onClick.AddListener(OnCloseBtnClicked);
            _createNewButton.onClick.AddListener(() =>
            {
                OnElementSelected(_newSaveField.text);
            });
        }
        
        public void Show()
        {
            _canvas.enabled = true;
            foreach (var save in _boardSerializer.GetAllSaves())
            {
                var item = Instantiate(_elementPrefab, _contentParent);
                item.gameObject.SetActive(true);
                _elements.Add(item);
                item.Init(new EditorElement.Data(save.CreationTime, save.FileName, OnElementSelected, OnElementDeleted));
            }
        }

        private void OnCloseBtnClicked()
        {
            _canvas.enabled = false;
            foreach (var element in _elements)
            {
                Destroy(element.gameObject);
            }
            _elements.Clear();
        }

        private void OnElementSelected(string name)
        {
            if(string.IsNullOrEmpty(name))
                return;
            var loadingOperations = new Queue<ILoadingOperation>();
            loadingOperations.Enqueue(new EditorGameLoadingOperation(name));
            LoadingScreen.Instance.Load(loadingOperations);
        }
        
        private void OnElementDeleted(EditorElement element)
        {
            _boardSerializer.Delete(element.FileName);
            _elements.Remove(element);
            Destroy(element.gameObject);
        }
    }
}