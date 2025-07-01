using UnityEngine;
using UnityEngine.Events;

public class EmptyEventListener : MonoBehaviour
{
    [SerializeField] private EmptyEventAsset _gameEventAsset;
    public UnityEvent OnGameEventInvoked;

    private void OnEnable()
    {
        _gameEventAsset.OnInvoked.AddListener(GameEventInvoked);
    }

    private void OnDisable()
    {
        _gameEventAsset.OnInvoked.RemoveListener(GameEventInvoked);
    }

    public void GameEventInvoked()
    {
        OnGameEventInvoked.Invoke();
    }
}
