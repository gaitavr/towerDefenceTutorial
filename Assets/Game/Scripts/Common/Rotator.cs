using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotator : MonoBehaviour
{
    [SerializeField]
    private float _speed;
    [SerializeField]
    private bool _isActive;

    private void Update()
    {
        if(_isActive == false)
            return;
        transform.Rotate(Vector3.forward * _speed);
    }

    public void Enable() => _isActive = true;
    public void Disable() => _isActive = false;
}
