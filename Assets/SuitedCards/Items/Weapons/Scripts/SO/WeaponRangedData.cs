using UnityEngine;

[CreateAssetMenu(fileName = "RangeWeaponData", menuName = "Scriptable Objects/WeaponData/WeaponRangedData")]
public class WeaponRangedData : WeaponData
{
    [Header("Ranged")]
    [field: SerializeField]  public ProjectileBase Projectile { get; private set; }
    [field: SerializeField] public int ShotCount { get; private set; } = 1;
    [field: SerializeField] public float Inaccuracy { get; private set; } = 1f;
    [field: SerializeField] public float Speed { get; private set; } = 10f;


}
