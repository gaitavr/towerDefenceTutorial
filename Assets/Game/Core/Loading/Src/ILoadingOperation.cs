using System;
using Cysharp.Threading.Tasks;

namespace Core.Loading
{
    public interface ILoadingOperation
    {
        string Description { get; }
        
        UniTask Load(Action<float> onProgress);
    }
}