using UnityEngine;

namespace Game.Core.GamePlay
{
    public class GamePlayUI : MonoBehaviour
    {
        [SerializeField] private Transform _visualizationSocket;
        [SerializeField] private Transform _detailsSocket;
        [SerializeField] private Transform _actionsSocket;
        
        private UnitVisualizationUI _currentVisualization;
        private UnitDetailsUI _currentDetails;
        private UnitActionsUI _currentActions;

        public Transform VisualizationSocket => _visualizationSocket;
        public Transform DetailsSocket => _detailsSocket;
        public Transform ActionsSocket => _actionsSocket;

        public void ShowUnit(UnitVisualizationUI visualizationUI,
            UnitDetailsUI detailsUI, UnitActionsUI actionsUI)
        {
            _currentVisualization = visualizationUI;
            _currentVisualization.Show();
            _currentDetails = detailsUI;
            _currentDetails.Show();
            _currentActions = actionsUI;
            _currentActions.Show();
        }

        public void Clear()
        {
            Destroy(_currentVisualization.gameObject);
            _currentVisualization = null;
            Destroy(_currentDetails.gameObject);
            _currentDetails = null;
            Destroy(_currentActions.gameObject);
            _currentActions = null;
        }
    }
}