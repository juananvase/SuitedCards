using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UIElements;

[CreateAssetMenu(menuName = "Game Event/EmptyEventAsset")]
public class EmptyEventAsset : ScriptableObject
{
    private List<UnityAction> _listeners = new();
    public UnityEvent OnInvoked;

    public void Invoke()
    {
        OnInvoked.Invoke();
    }

    public void AddListener(UnityAction listener)
    {
        OnInvoked.AddListener(listener);
        _listeners.Add(listener);
    }

    public void RemoveListener(UnityAction listener)
    {
        OnInvoked.RemoveListener(listener);
        if (_listeners.Contains(listener)) _listeners.Remove(listener);
    }
}
