using GamePlay.Modes;
using UnityEngine;

namespace GamePlay.Defend
{
    public class MortarTower : Tower
    {
        [SerializeField, Range(0.5f, 3f)] private float _shellBlastRadius = 1f;
        [SerializeField, Range(1f, 100f)] private float _damage;
        [Space]
        [SerializeField] private Transform _rotator;
        [SerializeField] private Transform _spawnPoint;

        private float _launchSpeed;
        private float _launchProgress;
        private float _shootsPerSeconds = 1f;

        private void Awake()
        {
            OnValidate();
        }

        private void OnValidate()
        {
            var x = _targetingRange + 0.251f;
            var y = -_spawnPoint.position.y;
            _launchSpeed = Mathf.Sqrt(9.81f * (y + Mathf.Sqrt(x * x + y * y)));
        }

        public override void Initialize(GameTileContentFactory factory, int level)
        {
            base.Initialize(factory, level);
            _shootsPerSeconds = OriginFactory.MortarConfig.GetShootsPerSecond(Level);
        }

        public override void GameUpdate()
        {
            _launchProgress += Time.deltaTime * _shootsPerSeconds;
            while (_launchProgress >= 1f)
            {
                if (IsAcquireTarget(out TargetPoint target))
                {
                    Launch(target);
                    _launchProgress -= 1f;
                }
                else
                {
                    _launchProgress = 0.999f;
                }
            }
        }

        private void Launch(TargetPoint target)
        {
            var launchPoint = _spawnPoint.position;
            var targetPoint = target.Position;
            targetPoint.y = 0f;

            Vector3 dir;
            dir.x = targetPoint.x - launchPoint.x;
            dir.y = 0;
            dir.z = targetPoint.z - launchPoint.z;

            var x = dir.magnitude;
            var y = -launchPoint.y;
            dir /= x;

            var g = 9.81f;
            var s = _launchSpeed;
            var s2 = s * s;

            var r = s2 * s2 - g * (g * x * x + 2f * y * s2);
            r = Mathf.Max(0, r);

            var tanTheta = (s2 + Mathf.Sqrt(r)) / (g * x);
            var cosTheta = Mathf.Cos(Mathf.Atan(tanTheta));
            var sinTheta = cosTheta * tanTheta;

            _rotator.localRotation = Quaternion.LookRotation(dir);

            QuickGameMode.SpawnShell().Initialize(launchPoint, targetPoint,
                new Vector3(s * cosTheta * dir.x, s * sinTheta, s * cosTheta * dir.z), _shellBlastRadius, _damage);
        }
    }
}
