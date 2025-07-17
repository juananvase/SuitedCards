using System;
using PrimeTween;
using UnityEngine;
using UnityEngine.EventSystems;

[RequireComponent(typeof(RectTransform))]
public class ItemCardBase : MonoBehaviour, IBeginDragHandler, IEndDragHandler, IDragHandler, IDropHandler, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private ItemBase _item;
    private AnchorType _itemAnchor => _item.ItemData.AnchorType;
    
    [SerializeField] private Canvas _hudCanvas;
    [SerializeField] private RectTransform _rectTransform;
    [SerializeField] private CanvasGroup _canvasGroup;

    private int _siblingIndex;
    private Vector3 _startPosition;

    [field: SerializeField] public ItemCardData ItemCardData { get; private set; }

    private void OnValidate()
    {
        _rectTransform = GetComponent<RectTransform>();
        _canvasGroup = GetComponent<CanvasGroup>();
    }

    private void OnEnable()
    {
        _hudCanvas = transform.root.GetComponent<Canvas>();
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
        _startPosition =  transform.position;
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
        if(!AssignItem()) transform.position =  _startPosition;
        
        ItemCardData.OnCardUsed?.Invoke(this);
    }

    private bool AssignItem()
    {
        if (SelectTarget() == null)
        {
            Debug.LogWarning("No target selected for item card drop");
            return false;
        }
        
        PlayerCharacter character = SelectTarget().GetComponent<PlayerCharacter>();
        if (character == null )
        {
            Debug.LogWarning("No character found on target object");
            return false;
        }

        ItemAnchor[] itemAnchors = character.ItemAnchors;
        if (itemAnchors.Length == 0)
        {
            Debug.LogError("No anchors found");
            return false;
        }

        for (int i = 0; i < itemAnchors.Length; i++)
        {
            if (itemAnchors[i].AnchorType == _itemAnchor && !itemAnchors[i].IsBeingUsed)
            {
                Instantiate(_item.gameObject, itemAnchors[i].transform.position, Quaternion.identity, itemAnchors[i].transform);
                ItemCardData.OnFindItems?.Invoke(character.gameObject);
                return true;
            }
        }
        
        return false;
        
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
