using UnityEngine;
using UnityEngine.Events;

public abstract class GameEventListener<T> : MonoBehaviour
{
    [SerializeField] private GameEventAsset<T> _gameEventAsset;
    public UnityEvent<T> OnGameEventInvoked;

    private void OnEnable()
    {
        _gameEventAsset.OnInvoked.AddListener(GameEventInvoked);
    }

    private void OnDisable()
    {
        _gameEventAsset.OnInvoked.RemoveListener(GameEventInvoked);
    }

    public void GameEventInvoked(T param) 
    {
        OnGameEventInvoked.Invoke(param);
    }
}
