using UnityEngine;

namespace GamePlay.Defend
{
    public class LaserTower : Tower
    {
        [SerializeField] private Transform _turret;
        [SerializeField] private Transform _laserBeam;
        [Space]
        [SerializeField, ColorUsage(true, true)] private Color _laserBeamColor;

        private Vector3 _laserBeamScale;
        private Vector3 _laserBeamStartPosition;
        private TargetPoint _target;
        private float _damagePerSecond;

        private void Awake()
        {
            _laserBeamScale = _laserBeam.localScale;
            _laserBeamStartPosition = _laserBeam.localPosition;
            SetLaserColor();
        }

        public override void Initialize(GameTileContentFactory factory, int level)
        {
            base.Initialize(factory, level);
            _damagePerSecond = OriginFactory.LaserConfig.GetDamagePerSecond(Level);
        }

        private void SetLaserColor()
        {
            var meshRenderer = _laserBeam.GetComponent<MeshRenderer>();
            meshRenderer.material.SetColor("_EmissionColor", _laserBeamColor);
        }

        public override void GameUpdate()
        {
            if (IsTargetTracked(ref _target) || IsAcquireTarget(out _target))
                ProcessShoot();
            else
                _laserBeam.localScale = Vector3.zero;
        }

        private void ProcessShoot()
        {
            var point = _target.Position;
            _turret.LookAt(point);
            _laserBeam.localRotation = _turret.localRotation;

            var distance = Vector3.Distance(_turret.position, point);
            _laserBeamScale.z = distance;
            _laserBeam.localScale = _laserBeamScale;
            _laserBeam.localPosition = _laserBeamStartPosition
                + 0.5f * distance * _laserBeam.forward;

            _target.Enemy.TakeDamage(_damagePerSecond * Time.deltaTime);
        }
    }
}
