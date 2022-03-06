using System;
using Unity.Collections;
using UnityEngine;
using UnityEngine.Jobs;

public struct TileCheckerJob : IJobParallelForTransform
{
    private readonly NativeArray<Vector3> _tilePositions;

    public TileCheckerJob(NativeArray<Vector3> tilePositions)
    {
        _tilePositions = tilePositions;
    }
    
    public void Execute(int index, TransformAccess transform)
    {
        for (int i = 0; i < _tilePositions.Length; i++)
        {
            if((transform.position - _tilePositions[i]).magnitude < 0.5f)
                Debug.LogError(transform.position);
        }
    }
}