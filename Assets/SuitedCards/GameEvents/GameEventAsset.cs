using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(menuName = "Game Event")]
public abstract class GameEventAsset<T> : ScriptableObject
{
    private List<UnityAction<T>> _listeners = new();
    public UnityEvent<T> OnInvoked;

    public void Invoke(T param) 
    {
        OnInvoked.Invoke(param);
    }

    public void AddListener(UnityAction<T> listener) 
    {
        OnInvoked.AddListener(listener);
        _listeners.Add(listener);
    }

    public void RemoveListener(UnityAction<T> listener)
    {
        OnInvoked.RemoveListener(listener);
        if (_listeners.Contains(listener)) _listeners.Remove(listener);
    }
}
