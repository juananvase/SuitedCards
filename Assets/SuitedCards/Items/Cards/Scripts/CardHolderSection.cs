using System;
using UnityEngine;

[RequireComponent(typeof(RectTransform))]
public class CardHolderSection : MonoBehaviour
{
    [SerializeField] private RectTransform _rectTransform;
    [SerializeField] private ItemCardBase[] _cards;
    
    float cardWidth = 0;
    float panelSize = 0;

    private void OnValidate()
    {
        _rectTransform = GetComponent<RectTransform>();
    }

    private void Start()
    {
        if(!FindCards()) return;
        
        if (_cards[0].TryGetComponent(out RectTransform cardRectTransform))  cardWidth = cardRectTransform.rect.width;
        panelSize = (cardWidth) * _cards.Length;
        panelSize = Mathf.Clamp(panelSize, 0f, _rectTransform.rect.width - cardWidth);
        DisplayCards();
    }

    private void DisplayCards()
    {
        float spacing = panelSize / (_cards.Length - 1) ;
        for (int i = 0; i < _cards.Length; i++)
        {
            _cards[i].transform.position = new Vector2((_rectTransform.position.x/2) + spacing * i , _rectTransform.position.y);
        }
    }
    
    [ContextMenu(nameof(FindCards))]
    private bool FindCards()
    {
        if (_cards.Length == 0) return false;
        
        _cards = GetComponentsInChildren<ItemCardBase>();
        return true;
    }
}
