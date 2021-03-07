using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class MortarTower : Tower
{
    [SerializeField, Range(0.5f, 2f)]
    private float _shootsPerSeconds = 1f;

    [SerializeField, Range(0.5f, 3f)]
    private float _shellBlastRadius = 1f;

    [SerializeField, Range(1f, 100f)]
    private float _damage;

    [SerializeField]
    private Transform _rotator;
    [SerializeField]
    private Transform _spawnPoint;

    public override GameTileContentType Type => GameTileContentType.MortarTower;

    private float _launchSpeed;
    private float _launchProgress;

    private void Awake()
    {
        OnValidate();
    }

    private void OnValidate()
    {
        float x = _targetingRange + 0.251f;
        float y = -_spawnPoint.position.y;
        _launchSpeed = Mathf.Sqrt(9.81f * (y + Mathf.Sqrt(x * x + y * y)));
       
    }

    public override void GameUpdate()
    {
        _launchProgress += Time.deltaTime * _shootsPerSeconds;
        while (_launchProgress >=1f)
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
        Vector3 launchPoint = _spawnPoint.position;
        Vector3 targetPoint = target.Position;
        targetPoint.y = 0f;

        Vector3 dir;
        dir.x = targetPoint.x - launchPoint.x;
        dir.y = 0;
        dir.z = targetPoint.z - launchPoint.z;

        float x = dir.magnitude;
        float y = -launchPoint.y;
        dir /= x;

        float g = 9.81f;
        float s = _launchSpeed;
        float s2 = s * s;

        float r = s2 * s2 - g * (g * x * x + 2f * y * s2);
        r = Mathf.Max(0, r);
        
        float tanTheta = (s2 + Mathf.Sqrt(r)) / (g * x);
        float cosTheta = Mathf.Cos(Mathf.Atan(tanTheta));
        float sinTheta = cosTheta * tanTheta;
        
        _rotator.localRotation = Quaternion.LookRotation(dir);

        QuickGame.SpawnShell().Initialize(launchPoint, targetPoint, 
            new Vector3(s * cosTheta * dir.x, s * sinTheta, s * cosTheta * dir.z), _shellBlastRadius, _damage);
    }

}
