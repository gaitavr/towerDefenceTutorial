using Core;
using TMPro;
using UnityEngine;

namespace Core.UI
{
    public sealed class CurrenciesUI : MonoBehaviour
    {
        [SerializeField] private TMP_Text _crystalsLabel;
        [SerializeField] private TMP_Text _gasLabel;

        private IUserCurrenciesStateReadonly Currencies => ProjectContext.I.UserContainer.State.Currencies;

        public void Show()
        {
            Currencies.Changed += OnCurrenciesChanged;
            OnCurrenciesChanged();
        }

        private void OnCurrenciesChanged()
        {
            _crystalsLabel.text = Currencies.Crystals.ToString();
            _gasLabel.text = Currencies.Gas.ToString();
        }

        public void Hide()
        {
            Currencies.Changed -= OnCurrenciesChanged;
        }
    }
}
