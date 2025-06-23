using UnityEngine;

public class WeaponRangeProjectile : WeaponBase
{
    public WeaponRangedData WeaponRangedData => WeaponData as WeaponRangedData;
    [SerializeField] private Transform _muzzle;
    
    protected override void Attack(Vector3 aimPosition, GameObject instigator, int team)
    {
        base.Attack(aimPosition, instigator, team);
        
        Vector3 spawnPosition = _muzzle.position;
        Vector3 aimDirection = (aimPosition - spawnPosition).normalized;
        Quaternion spawnRotation = Quaternion.LookRotation(aimDirection);

        for (int i = 0; i < WeaponRangedData.ShotCount; i++)
        {
            float horizontalInaccuracy = Random.Range(-WeaponRangedData.Inaccuracy, WeaponRangedData.Inaccuracy);
            float verticalInaccuracy = Random.Range(-WeaponRangedData.Inaccuracy, WeaponRangedData.Inaccuracy);

            Vector3 leftRightAngle = _muzzle.up * horizontalInaccuracy;
            Vector3 upDownAngle = _muzzle.right * verticalInaccuracy;
            Quaternion inaccuracyRotation = Quaternion.Euler(leftRightAngle + upDownAngle);
            
            Quaternion finalRotation = spawnRotation * inaccuracyRotation;

            ProjectileBase spawnedProjectile = Instantiate(WeaponRangedData.Projectile, spawnPosition, finalRotation);
            spawnedProjectile.Launch(WeaponRangedData.Speed, WeaponRangedData.Range, WeaponRangedData.Damage, WeaponRangedData.DamageType, instigator, team);
        }
    }
}
