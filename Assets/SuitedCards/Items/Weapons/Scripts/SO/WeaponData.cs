using UnityEngine;

[CreateAssetMenu(fileName = "WeaponData", menuName = "Scriptable Objects/WeaponData/WeaponData")]
public class WeaponData : ScriptableObject
{
    [field: SerializeField] public float Damage { get; private set; } = 10f;
    [field: SerializeField] public float Range { get; private set; } = 5f;
    //TODO if necessary add effective range for AI purposes
    [field: SerializeField] public float AttackRate { get; private set; } = 2f;
    [field: SerializeField] public float AttackDuration { get; private set; } = 3f;
    [field: SerializeField] public DamageType DamageType { get; private set; } = DamageType.None;
    
}
