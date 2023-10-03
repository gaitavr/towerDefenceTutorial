using Cysharp.Threading.Tasks;

namespace Core.Communication
{
    public sealed class LocalPvpCommunicator : IPvpCommunicator
    {
        public UniTask<UserAttackScenarioState> GetAttackScenario()
        {
            throw new System.NotImplementedException();
        }

        public UniTask<UserBoardState> GetBoard()
        {
            throw new System.NotImplementedException();
        }
    }
}