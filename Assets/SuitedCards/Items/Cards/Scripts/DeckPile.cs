using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class DeckPile : CardPileBase
{
    [field: SerializeField] public List<ItemCardBase> Deck { get; private set; }
    [field: SerializeField] public List<ItemCardBase> CardsDealt { get; private set; }
    [field: SerializeField] public ItemCardBaseListEventAsset OnDealCards {get; private set; }
    [field: SerializeField] public EmptyEventAsset OnCardsNeeded { get; private set; }
    

    private void Start()
    {
        SpawnCards();
        ShuffleDeck();
        DealCards(5);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            DealCards(5);
        }
    }

    private void SpawnCards()
    {
        for (int i = 0; i < Deck.Count; i++)
        {
            //TODO Implement object pulling?
            CurrentCards.Add(Instantiate(Deck[i].gameObject, transform.position, Quaternion.identity, transform).GetComponent<ItemCardBase>());
        }
    }
    
    public void DealCards(int cardDealtCount)
    {
        if (CurrentCards.Count < cardDealtCount)
        {
            int remainingCards = cardDealtCount - CurrentCards.Count;
            
            DealCards(CurrentCards.Count);
            
            OnCardsNeeded?.Invoke();
            
            DealCards(remainingCards);
            return;
        }

        for (int i = cardDealtCount; i-- > 0;)
        {
            CardsDealt.Add(CurrentCards[i]);
            CurrentCards.RemoveAt(i);
        }
        
        OnDealCards.Invoke(CardsDealt);
        CardsDealt.Clear();
    }
    
    public void ShuffleDeck()
    {
        // Fisher–Yates shuffle
        
        ItemCardBase cardToSwap;
        ItemCardBase swappedCard;
        
        for (int i = 0; i < CurrentCards.Count; i++)
        {
            int j = Random.Range(i, CurrentCards.Count);
            
            cardToSwap = CurrentCards[j];
            swappedCard = CurrentCards[i];
            
            CurrentCards[i] = cardToSwap;
            CurrentCards[j] = swappedCard;
        }
    }
}
