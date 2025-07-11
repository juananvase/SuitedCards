using UnityEngine;

[CreateAssetMenu(fileName = "ItemCardData", menuName = "Scriptable Objects/ItemCardData")]
public class ItemCardData : ScriptableObject
{
    [field: SerializeField] public LayerMask  TargetMask {get; private set; }
    [field: SerializeField] public GameObject Item {get; private set; }
    [field: SerializeField] public GameObjectEventAsset OnFindWeapons {get; private set; }
}
