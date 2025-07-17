using System;
using System.Collections.Generic;
using UnityEngine;

public abstract class CardPileBase : MonoBehaviour
{
    [field: SerializeField] public List<ItemCardBase> CurrentCards { get ; protected set; }

    public virtual void ReceiveCards(List<ItemCardBase> cardsDealt)
    {
        for (int i = 0; i < cardsDealt.Count; i++)
        {
            CurrentCards.Add(cardsDealt[i]);
            cardsDealt[i].transform.SetParent(transform);
            cardsDealt[i].transform.localPosition = Vector3.zero;
            cardsDealt[i].transform.rotation =  Quaternion.identity;
        }
    }
    
    public virtual void RemoveCards(List<ItemCardBase> cardsRemoved)
    {
        for (int i = 0; i < cardsRemoved.Count; i++)
        {
            CurrentCards.Remove(cardsRemoved[i]);
        }
    }

    public virtual void ReceiveCard(ItemCardBase cardsDealt)
    {
        CurrentCards.Add(cardsDealt);
        cardsDealt.transform.SetParent(transform);
        cardsDealt.transform.localPosition = Vector3.zero;
    }
    
    public virtual void RemoveCard(ItemCardBase cardRemoved)
    {
        CurrentCards.Remove(cardRemoved);
    }
}
