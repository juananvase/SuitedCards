using System;
using PrimeTween;
using UnityEngine;
using UnityEngine.EventSystems;

[RequireComponent(typeof(RectTransform))]
public class ItemCardBase : MonoBehaviour, IBeginDragHandler, IEndDragHandler, IDragHandler, IDropHandler, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private ItemBase _item;
    private AnchorType _itemAnchor => _item.ItemData.AnchorType;
    
    //TODO get hud canvas from game manager
    [SerializeField] private Canvas _hudCanvas;
    
    [SerializeField] private RectTransform _rectTransform;
    [SerializeField] private CanvasGroup _canvasGroup;

    private int _siblingIndex;

    [field: SerializeField] public ItemCardData ItemCardData { get; private set; }

    private void OnValidate()
    {
        _rectTransform = GetComponent<RectTransform>();
        _canvasGroup = GetComponent<CanvasGroup>();
    }
    
    public void OnPointerEnter(PointerEventData eventData)
    {
        _siblingIndex = transform.GetSiblingIndex();
        transform.SetAsLastSibling();
        Tween.Scale(transform, startValue: Vector3.one , endValue: Vector3.one * 1.5f, duration: 0.5f);
    }
    
    public void OnPointerExit(PointerEventData eventData)
    {
        transform.SetSiblingIndex(_siblingIndex);
        Tween.Scale(transform, startValue: transform.localScale, endValue: Vector3.one, duration: 0.5f);
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        _canvasGroup.alpha = 0.6f;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        _canvasGroup.alpha = 1f;
    }

    public void OnDrag(PointerEventData eventData)
    {
        _rectTransform.anchoredPosition += eventData.delta / _hudCanvas.scaleFactor;
        
    }

    public void OnDrop(PointerEventData eventData)
    {
        
        AssignItem();
        //TODO send card to the grave pile
        Destroy(gameObject);
    }

    private void AssignItem()
    {
        if (SelectTarget().TryGetComponent(out PlayerCharacter character))
        {
            ItemAnchor[] itemAnchors = character.ItemAnchors;
            
            if (itemAnchors.Length == 0)
            {
                Debug.LogError("No anchors found");
                return;
            }

            for (int i = 0; i < itemAnchors.Length; i++)
            {
                if (itemAnchors[i].AnchorType == _itemAnchor && !itemAnchors[i].IsBeingUsed)
                {
                    Instantiate(_item.gameObject, itemAnchors[i].transform.position, Quaternion.identity, itemAnchors[i].transform);
                    ItemCardData.OnFindItems?.Invoke(character.gameObject);
                    return;
                }
            }
        }
    }

    private GameObject SelectTarget()
    {
        Ray mouseRay = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(mouseRay, out RaycastHit hitInfo, Mathf.Infinity, ItemCardData.TargetMask))
        {
            return hitInfo.collider.gameObject;
        }

        return null;
    }
    
}
