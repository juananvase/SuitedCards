using System;
using System.Collections;
using UnityEngine;

public class ItemAnchor : MonoBehaviour
{
    [field: SerializeField] public AnchorType AnchorType { get; private set; } = AnchorType.None;
    [SerializeField] public bool IsBeingUsed => transform.childCount > 0;

    private Coroutine _checkOccupy;
    
}

public enum AnchorType
{
    None,
    Head,
    Body,
    Hand,
    Leg,
    Feet,
}
