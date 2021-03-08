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
        private Button _deleteButton;//TODO implement

        private string _selectParameter;
        
        public void Init(Data data)
        {
            _timeText.text = data.Time;
            _nameText.text = data.Name;
            _selectParameter = data.Name;
            _selectButton.onClick.AddListener(() =>
            {
                data.Selected?.Invoke(_selectParameter);
            });
        }
        
        public class Data
        {
            public string Time;
            public string Name;
            public Action<string> Selected;

            public Data(string time, string name, Action<string> selected)
            {
                Time = time;
                Name = name;
                Selected = selected;
            }
        }
    }
}