using System.Collections.Generic;
using UnityEngine;

namespace GamePlay
{
    public class TargetPoint : MonoBehaviour
    {
        public Enemy Enemy { get; private set; }

        private bool _isEnabled;
        public bool IsEnabled
        {
            get => _isEnabled;
            set
            {
                _collider.enabled = value;
                _isEnabled = value;
            }
        }

        public Vector3 Position => transform.position;

        public float ColliderSize { get; private set; }

        private const int ENEMY_LAYER_MASK = 1 << 9;

        private static readonly Collider[] _buffer = new Collider[100];
        public static int BufferedCount { get; private set; }

        private SphereCollider _collider;

        private void Awake()
        {
            Enemy = transform.root.GetComponent<Enemy>();
            _collider = GetComponent<SphereCollider>();
            ColliderSize = _collider.radius * transform.localScale.x;
        }

        public static bool FillBufferInCapsule(Vector3 position, float range)
        {
            Vector3 top = position;
            top.y += 3f;
            BufferedCount = Physics.OverlapCapsuleNonAlloc(position, top, range, _buffer, ENEMY_LAYER_MASK);
            return BufferedCount > 0;
        }

        public static bool FillBufferInBox(Vector3 position, Vector3 halfSize)
        {
            BufferedCount = Physics.OverlapBoxNonAlloc(position, halfSize, _buffer,
                Quaternion.identity, ENEMY_LAYER_MASK);
            return BufferedCount > 0;
        }

        public static TargetPoint GetBuffered(int index)
        {
            var target = _buffer[index].GetComponent<TargetPoint>();
            return target;
        }

        public static IEnumerable<TargetPoint> TargetPoints()
        {
            if (BufferedCount <= 0)
                yield break;
            for (var i = 0; i < BufferedCount; i++)
            {
                yield return GetBuffered(i);
            }
        }
    }
}
