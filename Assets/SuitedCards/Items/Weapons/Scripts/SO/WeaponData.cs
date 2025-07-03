using UnityEngine;

[CreateAssetMenu(fileName = "WeaponData", menuName = "Scriptable Objects/WeaponData/WeaponData")]
public class WeaponData : ScriptableObject
{
    [field: SerializeField] public float Damage { get; private set; } = 10f;
    [field: SerializeField] public float CriticDamagePercentage { get; private set; } = 0.1f;
    [field: SerializeField] public float Range { get; private set; } = 5f;
    
    [field: SerializeField] public float AttackDuration { get; private set; } = 3f;
    [field: SerializeField] public DamageType DamageType { get; private set; } = DamageType.None;
    
}
