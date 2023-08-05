using UnityEngine;

namespace Game.Core.GamePlay
{
    public class GamePlayUI : MonoBehaviour
    {
        [SerializeField] private Transform _visualizationSocket;
        [SerializeField] private Transform _detailsSocket;
        [SerializeField] private Transform _actionsSocket;

        public Transform VisualizationSocket => _visualizationSocket;
        public Transform DetailsSocket => _detailsSocket;
        public Transform ActionsSocket => _actionsSocket;
    }
}