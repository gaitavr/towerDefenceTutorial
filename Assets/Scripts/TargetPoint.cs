using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetPoint : MonoBehaviour
{
    public Enemy Enemy { get; private set; }
    public Vector3 Position => transform.position;

    public float ColliderSize { get; private set; }

    private void Awake()
    {
        Enemy = transform.root.GetComponent<Enemy>();
        ColliderSize = GetComponent<SphereCollider>().radius * transform.localScale.x;
    }
}
