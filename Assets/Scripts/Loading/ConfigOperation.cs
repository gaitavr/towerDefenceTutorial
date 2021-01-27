using System;
using System.Threading.Tasks;
using AppInfo;
using Random = UnityEngine.Random;

namespace Loading
{
    public class ConfigOperation : ILoadingOperation
    {
        public string Description => "Configuration loading...";
        
        public ConfigOperation(AppInfoContainer appInfoContainer)
        {
            
        }
        
        public async Task Load(Action<float> onProgress)
        {
            var loadTime = Random.Range(1.5f, 2.5f);
            const int steps = 4;
            
            for (var i = 1; i <= steps; i++)
            {
                await Task.Delay(TimeSpan.FromSeconds(loadTime/steps));
                onProgress?.Invoke(i / loadTime);
            }
            onProgress?.Invoke(1f);
        }
    }
}