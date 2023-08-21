using Cysharp.Threading.Tasks;
using System;
using System.Collections.Generic;

namespace Core.Loading
{
    public sealed class ParallelOperation : ILoadingOperation
    {
        private readonly string _description;
        private readonly IEnumerable<ILoadingOperation> _nestedOperations;

        public string Description => _description;

        public ParallelOperation(string description, IEnumerable<ILoadingOperation> nestedOperations)
        {
            _description = description;
            _nestedOperations = nestedOperations;
        }

        public UniTask Load(Action<float> onProgress)
        {
            return UniTask.WhenAll(_nestedOperations.Select(o => o.Load(onProgress)));
        }
    }
}
