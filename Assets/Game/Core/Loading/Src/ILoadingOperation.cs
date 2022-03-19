using System;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;

namespace Loading
{
    public interface ILoadingOperation
    {
        string Description { get; }
        
        UniTask Load(Action<float> onProgress);
    }
}