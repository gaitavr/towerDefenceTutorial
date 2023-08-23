using UnityEngine;

namespace GamePlay
{
    public class Shell : WarEntity
    {
        private Vector3 _launchPoint, _targetPoint, _launchVelocity;
        private float _age;
        private float _blastRadius, _damage;

        public void Initialize(Vector3 launchPoint, Vector3 targetPoint, Vector3 launchVelocity, float blastRadius, float damage)
        {
            _launchPoint = launchPoint;
            _targetPoint = targetPoint;
            _launchVelocity = launchVelocity;
            _blastRadius = blastRadius;
            _damage = damage;
        }

        public override bool GameUpdate()
        {
            _age += Time.deltaTime;
            var p = _launchPoint + _launchVelocity * _age;
            p.y -= 0.5f * 9.81f * _age * _age;

            if (p.y <= 0f)
            {
                QuickGameMode.SpawnExplosion().Initialize(_targetPoint, _blastRadius, _damage);
                OriginFactory.Reclaim(this);
                return false;
            }

            transform.localPosition = p;

            var d = _launchVelocity;
            d.y -= 9.81f * _age;
            transform.localRotation = Quaternion.LookRotation(d);

            QuickGameMode.SpawnExplosion().Initialize(p, 0.1f);

            return true;
        }
    }
}
