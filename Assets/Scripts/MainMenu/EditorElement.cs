using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace MainMenu
{
    public class EditorElement : MonoBehaviour
    {
        [SerializeField]
        private TextMeshProUGUI _timeText;
        [SerializeField]
        private TextMeshProUGUI _nameText;
        [SerializeField]
        private Button _selectButton;
        [SerializeField]
        private Button _deleteButton;

        public string FileName { get; private set; }
        
        public void Init(Data data)
        {
            _timeText.text = data.Time;
            _nameText.text = data.Name;
            FileName = data.Name;
            _selectButton.onClick.AddListener(() =>
            {
                data.Selected?.Invoke(FileName);
            });
            _deleteButton.onClick.AddListener(() =>
            {
                data.Deleted?.Invoke(this);
            });
        }
        
        public class Data
        {
            public string Time { get; private set; }
            public string Name { get; private set; }
            public Action<string> Selected { get; private set; }
            public Action<EditorElement> Deleted { get; private set; }

            public Data(string time, string name, Action<string> selected, Action<EditorElement> deleted)
            {
                Time = time;
                Name = name;
                Selected = selected;
                Deleted = deleted;
            }
        }
    }
}