using Cysharp.Threading.Tasks;
using System.IO;
using UnityEngine;

namespace Core.Communication
{
    public sealed class LocalUserStateCommunicator : IUserStateCommunicator
    {
        private string Path => $"{Application.persistentDataPath}/userAccountState.def";

        public UniTask<bool> SaveUserState(UserAccountState state)
        {
            var writeBytes = state.Serialize();
            File.WriteAllBytes(Path, writeBytes);
            return UniTask.FromResult(true);
        }
    }
}
