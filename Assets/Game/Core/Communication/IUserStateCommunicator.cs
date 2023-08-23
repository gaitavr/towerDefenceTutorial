using Cysharp.Threading.Tasks;

namespace Core.Communication
{
    public interface IUserStateCommunicator
    {
        UniTask<bool> SaveUserState(UserAccountState state);
    }
}
