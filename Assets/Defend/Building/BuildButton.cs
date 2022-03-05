using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class BuildButton : MonoBehaviour, IPointerDownHandler
{
    [SerializeField]
    private GameTileContentType _type;

    private Action<GameTileContentType> _listenerAction;

    public void AddListener(Action<GameTileContentType> listenerAction)
    {
        _listenerAction = listenerAction;
    }

    void IPointerDownHandler.OnPointerDown(PointerEventData eventData)
    {
        _listenerAction?.Invoke(_type);
    }
}