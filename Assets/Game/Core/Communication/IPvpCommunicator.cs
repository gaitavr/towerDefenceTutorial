using Cysharp.Threading.Tasks;

namespace Core.Communication
{
    public interface IPvpCommunicator
    {
        UniTask<UserAttackScenarioState> GetAttackScenario();
        UniTask<UserBoardState> GetBoard();
    }
}