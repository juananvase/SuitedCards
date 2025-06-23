using System;
using Unity.VisualScripting;
using UnityEngine;

[CreateAssetMenu(fileName = "CharacterData", menuName = "Scriptable Objects/CharacterData")]
public class CharacterData : ScriptableObject
{ 
    [Header("Health")]
    [field: SerializeField] public float MaxHealth { get; private set; } = 100f;
    
    [Header("Dash")]
    [field: SerializeField] public float DashRate { get; private set; } = 3f;
    
    [Header("Parry")]
    [field: SerializeField] public float ParryRate { get; private set; } = 3f;
}
