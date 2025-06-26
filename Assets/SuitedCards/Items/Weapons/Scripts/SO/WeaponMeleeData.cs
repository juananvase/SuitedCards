using UnityEngine;

[CreateAssetMenu(fileName = "WeaponMeleeData", menuName = "Scriptable Objects/WeaponData/WeaponMeleeData")]
public class WeaponMeleeData : WeaponData
{
    [Header("Attack")]
    [field: SerializeField] public float AttackAngle { get; private set; } = 120f;
    [field: SerializeField] public LayerMask AttackHitMask { get; private set; }
    
    [Header("Parry")]
    [field: SerializeField] public float ParryRate { get; private set; } = 3f;
    [field: SerializeField] public float ParryAttackWindow { get; private set; } = 0.2f;
    [field: SerializeField] public float ParryEfficiency { get; private set; } = 0.3f;
    [field: SerializeField] public LayerMask ParryHitMask { get; private set; }
}
