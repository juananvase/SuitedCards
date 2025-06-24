using UnityEngine;

public class WeaponMelee : WeaponBase
{
    public WeaponMeleeData WeaponMeleeData => WeaponData as WeaponMeleeData;
    private Collider[] _hits =  new Collider[5];
    
    //TODO fix animations and advance melee combat
    protected override void Attack(Vector3 aimPosition, GameObject instigator, int team)
    {
        base.Attack(aimPosition, instigator, team);
        
        Vector3 origin = instigator.transform.position;
        Vector3 direction = (aimPosition - origin).normalized;

        int hitCount = Physics.OverlapSphereNonAlloc(origin, WeaponMeleeData.Range, _hits, WeaponMeleeData.HitMask);

        for (var i = 0; i < hitCount; i++)
        {
            var hit = _hits[i];
            if (hit.gameObject == instigator) continue;

            //TODO decide how to manage friendly fire
            if (hit.TryGetComponent(out ITargetable target) && target.Team == team) continue;

            Vector3 targetDirection = (hit.transform.position - origin).normalized;
            float angleToHit = Vector3.Angle(direction, targetDirection);
            if (angleToHit > WeaponMeleeData.AttackAngle) continue;

            if (hit.TryGetComponent(out IDamageable damageable))
            {
                DamageInfo damageInfo = new DamageInfo(WeaponMeleeData.Damage, hit.gameObject, gameObject, instigator, WeaponMeleeData.DamageType);
                damageable.Damage(damageInfo);
            }
        }
    }
}
