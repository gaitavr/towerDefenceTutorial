using Cysharp.Threading.Tasks;
using System;
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

        public async UniTask<UserAccountState> GetUserState()
        {
            var result = new UserAccountState();
            var path = Path;
            if (File.Exists(path))
            {
                var readBytes = File.ReadAllBytes(path);
                result.Deserialize(readBytes);
            }
            await UniTask.Delay(TimeSpan.FromSeconds(0.5f));
            return result;
        }
    }
}
