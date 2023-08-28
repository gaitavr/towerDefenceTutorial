using UnityEngine;

namespace GamePlay.Defend
{
    public class LavaObstacleView : MonoBehaviour
    {
        [SerializeField] private Renderer _renderer;
        [SerializeField, Range(0.0f, 1.0f)] private float _speed;

        private Vector2 _offset;
        private static Vector2? _moveDirection;

        private void Awake()
        {
            if(_moveDirection == null)
                _moveDirection = new Vector2(Random.Range(-1f, 1f), Random.Range(-1f, 1f));
        }

        private void Update()
        {
            _offset += _moveDirection.Value * Time.deltaTime * _speed;
            _renderer.material.SetTextureOffset("_BaseMap", _offset);
        }
    }
}
