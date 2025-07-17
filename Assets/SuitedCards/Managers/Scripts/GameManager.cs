using System;
using UnityEngine;

[RequireComponent(typeof(TimeManager))]
[RequireComponent(typeof(QTEManager))]
public class GameManager : MonoBehaviour
{
    public static GameManager instance { get; private set; }

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(this);
        }
        else
        {
            instance = this;
        }
    }

    [field: SerializeField] public TimeManager TimeManager { get; private set; }
    [field: SerializeField] public QTEManager QTEManager { get; private set; }
    
    [field: SerializeField] public DeckPile DeckPile { get; private set; }
    [field: SerializeField] public GravePile GravePile { get; private set; }
    [field: SerializeField] public EmptyEventAsset OnCardsNeeded { get; private set; }

    private void OnValidate()
    {
        QTEManager = GetComponent<QTEManager>();
        TimeManager = GetComponent<TimeManager>();
    }

    private void OnEnable()
    {
        OnCardsNeeded.AddListener(TakeCardsFormGrave);
    }

    private void OnDisable()
    {
        OnCardsNeeded.RemoveListener(TakeCardsFormGrave);
    }

    private void TakeCardsFormGrave()
    {
        DeckPile.ReceiveCards(GravePile.CurrentCards);
        GravePile.CurrentCards.Clear();
    }

}
