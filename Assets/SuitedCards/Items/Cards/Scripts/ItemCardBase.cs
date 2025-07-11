using System;
using UnityEngine;
using UnityEngine.EventSystems;

[RequireComponent(typeof(RectTransform))]
public class ItemCardBase : MonoBehaviour, IPointerDownHandler, IBeginDragHandler, IEndDragHandler, IDragHandler, IDropHandler
{
    [SerializeField] private ItemBase _item;
    private AnchorType _itemAnchor => _item.ItemData.AnchorType;
    
    //TODO get hud canvas from game manager
    [SerializeField] private Canvas _canvas;
    [SerializeField] private RectTransform _rectTransform;

    //private GameObject _target = null;

    [field: SerializeField] public ItemCardData ItemCardData { get; private set; }

    private void OnValidate()
    {
        _rectTransform = GetComponent<RectTransform>();
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        
    }

    public void OnDrag(PointerEventData eventData)
    {
        _rectTransform.anchoredPosition += eventData.delta / _canvas.scaleFactor;
        
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
