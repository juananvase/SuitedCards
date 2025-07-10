using System;
using UnityEngine;
using UnityEngine.Splines;

[RequireComponent(typeof(RectTransform))]
public class CardHolderSection : MonoBehaviour
{
    [SerializeField] private RectTransform _rectTransform;
    [SerializeField] private ItemCardBase[] _cards;
    [SerializeField] private SplineContainer _splineContainer;

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
        //DisplayCards();
        UpdateHandDisplay();
    }

    private void DisplayCards()
    {
        float spacing = panelSize / (_cards.Length - 1) ;
        for (int i = 0; i < _cards.Length; i++)
        {
            _cards[i].transform.position = new Vector2((_rectTransform.position.x/2) + spacing * i , _rectTransform.position.y);
        }
    }

    private void UpdateHandDisplay() 
    {

        float cardSpacing = 1f / 10f;
        float firstCardPosition = 0.5f - (_cards.Length - 1) * cardSpacing / 2f;

        Spline spline = _splineContainer.Spline;

        for (int i = 0; i < _cards.Length; i++)
        {
            float p = firstCardPosition + i * cardSpacing;
            Vector3 splinePosition = spline.EvaluatePosition(p);
            Vector3 forward = spline.EvaluateTangent(p);
            Vector3 up = spline.EvaluateUpVector(p);

            Quaternion rotation = Quaternion.LookRotation(-up, Vector3.Cross(-up, forward).normalized);

            _cards[i].transform.position = splinePosition + transform.position + 0.01f * i * Vector3.back;
            _cards[i].transform.rotation = rotation;
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
