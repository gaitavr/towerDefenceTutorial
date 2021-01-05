using System;
using System.Threading.Tasks;
using Random = UnityEngine.Random;

namespace Loading
{
    public class UpdateOperation : ILoadingOperation
    {
        public string GetName => "Checking for updates...";
        
        public async Task Load(Action<float> onProgress)
        {
            var loadTime = Random.Range(1.5f, 2.5f);
            const int steps = 4;
            
            for (var i = 1; i <= steps; i++)
            {
                await Task.Delay(TimeSpan.FromSeconds(loadTime/steps));
                onProgress?.Invoke(i / loadTime);
            }
            onProgress?.Invoke(1f);;
        }
    }
}