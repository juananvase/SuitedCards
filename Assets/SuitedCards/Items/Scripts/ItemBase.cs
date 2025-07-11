using System.Collections;using UnityEngine;

public abstract class ItemBase : MonoBehaviour
{
    [field: SerializeField] public ItemData ItemData { get; private set; }
}
