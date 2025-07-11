using UnityEngine;

public abstract class ItemData : ScriptableObject
{
    [field: SerializeField] public AnchorType AnchorType { get; private set; } = AnchorType.None;
}
