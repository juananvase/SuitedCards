using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class GravePile : CardPileBase
{
    [field: SerializeField] public ItemCardBaseEventAsset OnCardUsed {get; private set; }
    [field: SerializeField] public ItemCardBaseListEventAsset OnCardsDiscarded {get; private set; }

    private void OnEnable()
    {
        OnCardUsed.AddListener(ReceiveCard);
        OnCardsDiscarded.AddListener(ReceiveCards);
    }

    private void OnDisable()
    {
        OnCardUsed.RemoveListener(ReceiveCard);
        OnCardsDiscarded.RemoveListener(ReceiveCards);
    }

    public override void ReceiveCard(ItemCardBase cardsDealt)
    {
        base.ReceiveCard(cardsDealt);
        cardsDealt.transform.rotation = Quaternion.AngleAxis(Random.Range(30,-30), Vector3.forward);
    }

    public override void ReceiveCards(List<ItemCardBase> cardsDealt)
    {
        base.ReceiveCards(cardsDealt);
        
        for (int i = 0; i < cardsDealt.Count; i++)
        {
            cardsDealt[i].transform.rotation = Quaternion.AngleAxis(Random.Range(30,-30), Vector3.forward);
        }
    }
}
