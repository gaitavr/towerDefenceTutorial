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
            var offset = 0;
            var lenght = state.GetLenght();
            var writeBytes = new byte[lenght];
            state.Serialize(writeBytes, ref offset);
            File.WriteAllBytes(Path, writeBytes);
            return UniTask.FromResult(true);
        }

        public async UniTask<UserAccountState> GetUserState()
        {
            var result = new UserAccountState();
            var path = Path;
            if (File.Exists(path))
            {
                var readBytes = await File.ReadAllBytesAsync(path);
                var offset = 0;
                result.Deserialize(readBytes, ref offset);
            }
            await UniTask.Delay(TimeSpan.FromSeconds(0.5f));
            return result;
        }
    }
}
