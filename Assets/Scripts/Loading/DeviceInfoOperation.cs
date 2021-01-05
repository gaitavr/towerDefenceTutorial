using System;
using System.Threading.Tasks;
using Random = UnityEngine.Random;

namespace Loading
{
    public class DeviceInfoOperation : ILoadingOperation
    {
        public string GetName => "Device data retrieving...";
        
        public async Task Load(Action<float> onProgress)
        { 
            var loadTime = Random.Range(1.1f, 1.4f);
            const int steps = 3;
            
            for (var i = 1; i <= steps; i++)
            {
                await Task.Delay(TimeSpan.FromSeconds(loadTime/steps));
                onProgress?.Invoke(i / loadTime);
            }
            onProgress?.Invoke(1f);
        }
    }
}