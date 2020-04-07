using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Game : MonoBehaviour
{
    [SerializeField]
    private Vector2Int _boardSize;

    [SerializeField]
    private GameBoard _board;

    private void Start()
    {
        _board.Initialize(_boardSize);
    }
}
