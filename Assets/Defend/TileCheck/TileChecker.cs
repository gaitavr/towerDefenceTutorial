using System;
using Unity.Collections;
using UnityEngine;
using UnityEngine.Jobs;

public class TileChecker : MonoBehaviour
{
    private GameBoard _board;
    private GameBehaviorCollection _enemies;

    private bool _isInited;
    private Vector3[] _tilePositions;
    
    public void Init(GameBoard board, GameBehaviorCollection enemies)
    {
        _board = board;
        _enemies = enemies;
        _tilePositions = _board.GetAllTilePositions;
        _isInited = true;
    }

    private void Update()
    {
        if(_isInited == false)
            return;

        var tilePositions = new NativeArray<Vector3>(_tilePositions, Allocator.TempJob);
        var tileCheckerJob = new TileCheckerJob(tilePositions);
        var enemiesTransform = new TransformAccessArray(_enemies.GetAllTransforms());
        var tileCheckerHandle = tileCheckerJob.Schedule(enemiesTransform);
        tileCheckerHandle.Complete();
        enemiesTransform.Dispose();
        tilePositions.Dispose();
    }
}