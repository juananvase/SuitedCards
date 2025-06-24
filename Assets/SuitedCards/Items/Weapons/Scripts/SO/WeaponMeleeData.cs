using UnityEngine;

[CreateAssetMenu(fileName = "WeaponMeleeData", menuName = "Scriptable Objects/WeaponData/WeaponMeleeData")]
public class WeaponMeleeData : WeaponData
{
    [field: SerializeField] public float AttackAngle { get; private set; } = 120f;
    [field: SerializeField] public LayerMask HitMask { get; private set; }
}
