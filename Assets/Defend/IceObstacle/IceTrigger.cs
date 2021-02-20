using System;
using UnityEngine;

[RequireComponent(typeof(Collider), typeof(Rigidbody))]
public class IceTrigger : MonoBehaviour
{
    public event Action<TargetPoint> Entered;
    public event Action<TargetPoint> Exited;
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out TargetPoint targetPoint))
        {
            Entered?.Invoke(targetPoint);
        }
    }
    
    private void OnTriggerExit(Collider other)
    {
        if (other.TryGetComponent(out TargetPoint targetPoint))
        {
            Exited?.Invoke(targetPoint);
        }
    }
}