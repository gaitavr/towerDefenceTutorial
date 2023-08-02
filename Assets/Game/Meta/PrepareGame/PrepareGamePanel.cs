using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;

public class PrepareGamePanel : MonoBehaviour
{
    [SerializeField] private GameObject[] _colors;
    [SerializeField] private GameObject _go;
    [SerializeField] private Vector3 _defaultScale;
    [SerializeField] private Vector3 _bigScale;
    
    public async UniTask<bool> Prepare(float seconds, CancellationToken cancellationToken)
    {
        Reset();
        gameObject.SetActive(true);
        
        var elementsCount = _colors.Length + 1;
        var unitTime = seconds / elementsCount;

        for (var i = 0; i < _colors.Length; i++)
        {
            if(i > 0)
                _colors[i - 1].transform.localScale = _defaultScale;
            _colors[i].transform.localScale = _bigScale;
            await UniTask.Delay(TimeSpan.FromSeconds(unitTime), cancellationToken: cancellationToken)
                .SuppressCancellationThrow();
            if (cancellationToken.IsCancellationRequested)
            {
                gameObject.SetActive(false);
                return false;
            }
        }
        
        foreach (var c in _colors)
        {
            c.gameObject.SetActive(false);
        }
        _go.SetActive(true);
        await UniTask.Delay(TimeSpan.FromSeconds(unitTime), cancellationToken: cancellationToken)
            .SuppressCancellationThrow();
        if (cancellationToken.IsCancellationRequested)
        {
            gameObject.SetActive(false);
            return false;
        }
        
        gameObject.SetActive(false);
        return true;
    }

    private void Reset()
    {
        foreach (var c in _colors)
        {
           c.transform.localScale = _defaultScale;
           c.gameObject.SetActive(true);
        }
        _go.SetActive(false);
    }
}
