using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Splines;

[RequireComponent(typeof(RectTransform))]
public class HandPile : CardPileBase
{
    [field: SerializeField] public int CardCapacity { get; private set; } = 10;
    [field: SerializeField] public ItemCardBaseListEventAsset OnDealCards {get; private set; }
    [field: SerializeField] public ItemCardBaseEventAsset OnCardUsed {get; private set; }
    [field: SerializeField] public ItemCardBaseListEventAsset OnCardsDiscarded {get; private set; }
    [SerializeField] private SplineContainer _splineContainer;

    private void OnEnable()
    {
        OnDealCards.AddListener(ReceiveCards);
        OnCardUsed.AddListener(RemoveCard);
    }

    private void OnDisable()
    {
        OnDealCards.RemoveListener(ReceiveCards);
        OnCardUsed.RemoveListener(RemoveCard);
    }

    public override void ReceiveCards(List<ItemCardBase> cardsDealt)
    {
        if (CurrentCards.Count >= CardCapacity)
        {
            //TODO make character said "I can not have that many items" or something similar
            Debug.LogWarning("Hand its at max capacity");

            for (int i = cardsDealt.Count; i-- > 0;)
            {
                OnCardUsed?.Invoke(cardsDealt[i]);
            }
            return;
        }

        if ((CurrentCards.Count + cardsDealt.Count) >= CardCapacity)
        {
            int missingCards = CardCapacity - CurrentCards.Count;
            
            for (int i = missingCards; i-- > 0;)
            {
                ReceiveCard(cardsDealt[i]);
                cardsDealt.RemoveAt(i);
            }
            
            OnCardsDiscarded?.Invoke(cardsDealt);
            
            return;
        }

        base.ReceiveCards(cardsDealt);
        UpdateHandDisplay();
    }

    public override void ReceiveCard(ItemCardBase cardsDealt)
    {
        base.ReceiveCard(cardsDealt);
        UpdateHandDisplay();
    }

    public override void RemoveCard(ItemCardBase cardRemoved)
    {
        base.RemoveCard(cardRemoved);
        UpdateHandDisplay();
    }

    private void UpdateHandDisplay() 
    {
        float cardSpacing = 1f / 10f;
        float firstCardPosition = 0.5f - (CurrentCards.Count - 1) * cardSpacing / 2f;

        Spline spline = _splineContainer.Spline;

        for (int i = 0; i < CurrentCards.Count; i++)
        {
            float p = firstCardPosition + i * cardSpacing;
            Vector3 splinePosition = spline.EvaluatePosition(p);
            Vector3 forward = spline.EvaluateTangent(p);
            Vector3 up = spline.EvaluateUpVector(p);

            Quaternion rotation = Quaternion.LookRotation(-up, Vector3.Cross(-up, forward).normalized);

            CurrentCards[i].transform.position = splinePosition + transform.position + 0.01f * i * Vector3.back;
            CurrentCards[i].transform.rotation = rotation;
        }
    }
}
