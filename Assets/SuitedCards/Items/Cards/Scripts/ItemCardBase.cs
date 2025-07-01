using System;
using UnityEngine;
using UnityEngine.EventSystems;

[RequireComponent(typeof(RectTransform))]
public class ItemCardBase : MonoBehaviour, IPointerDownHandler, IBeginDragHandler, IEndDragHandler, IDragHandler, IDropHandler
{
    //TODO get hud canvas from game manager
    [SerializeField] private Canvas _canvas;
    [SerializeField] private RectTransform _rectTransform;

    private GameObject _target = null;

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
        //TODO keep this function empty -> WeaponCard, ArmorCard, EffectCard
        if (SelectTarget().TryGetComponent(out CharacterBase character))
        {
            if (character.WeaponAnchors.Length == 0)
            {
                Debug.LogError("No weapon anchors found");
                return;
            }

            Instantiate(ItemCardData.Item, character.WeaponAnchors[0].position, Quaternion.identity, character.WeaponAnchors[0]);
            ItemCardData.OnFindWeapons?.Invoke(character.gameObject);
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
