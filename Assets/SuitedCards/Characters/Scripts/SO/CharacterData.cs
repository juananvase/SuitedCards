using System;
using Unity.VisualScripting;
using UnityEngine;

[CreateAssetMenu(fileName = "CharacterData", menuName = "Scriptable Objects/CharacterData")]
public class CharacterData : BaseData
{ 
    [Header("Dash")]
    [field: SerializeField] public float DashRate { get; private set; } = 3f;
    [field: SerializeField] public float DashMultiplier { get; private set; } = -7f;
    [field: SerializeField] public float DashYOffset { get; private set; } = 1f;
    [field: SerializeField] public float DashDuration { get; private set; } = 0.5f;
    
}
