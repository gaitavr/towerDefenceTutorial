using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Core.Communication
{
    public sealed class LocalPvpCommunicator : IPvpCommunicator
    {
        public UniTask<UserAttackScenarioState> GetAttackScenario()
        {
            return UniTask.FromResult(UserAttackScenarioState.GetInitial());
        }

        public UniTask<UserBoardState> GetBoard()
        {
            var size = new Vector2Int(10, 10);
            var board = UserBoardState.GetInitial(size, "dummy");
           
            for (var y = 1; y < 9; y++)
            {
                for (var x = 1; x < 9; x++)
                {
                    var content = GameTileContentType.Empty;
                    var rnd = Random.Range(0, 6);
                    switch (rnd)
                    {
                        case 0:
                            content = GameTileContentType.Wall;
                            break;
                        case 1:
                            content = GameTileContentType.LaserTower;
                            break;
                        case 2:
                            content = GameTileContentType.MortarTower;
                            break;
                        case 3:
                            content = GameTileContentType.Ice;
                            break;
                        case 4:
                            content = GameTileContentType.Lava;
                            break;
                    }

                    board.Content[size.x * y + x] = content;
                }
            }
            return UniTask.FromResult(board);
        }
    }
}