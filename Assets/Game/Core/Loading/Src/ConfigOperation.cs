using System;
using Core;
using Cysharp.Threading.Tasks;

namespace Core.Loading
{
    public sealed class ConfigOperation : ILoadingOperation
    {
        public string Description => "Configuration loading...";
        
        public async UniTask Load(Action<float> onProgress)
        {
            var loadTime = 0.5f;
            const int steps = 4;
            
            for (var i = 1; i <= steps; i++)
            {
                await UniTask.Delay(TimeSpan.FromSeconds(loadTime/steps));
                onProgress?.Invoke(i / loadTime);
            }

            ProjectContext.I.UserContainer.Configuration = new AppConfiguration();

            onProgress?.Invoke(1f);
        }
    }
}