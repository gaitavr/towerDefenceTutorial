using System;
using System.Threading.Tasks;

namespace Loading
{
    public interface ILoadingOperation
    {
        string GetName { get; }
        
        Task Load(Action<float> onProgress);
    }
}