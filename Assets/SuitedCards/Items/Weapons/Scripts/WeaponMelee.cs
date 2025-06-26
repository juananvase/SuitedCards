using System;
using UnityEngine;

public class WeaponMelee : WeaponBase, IParryable
{
    public WeaponMeleeData WeaponMeleeData => WeaponData as WeaponMeleeData;
    public float ParryEfficiency { get; set; }
    private float _lastParryTime;
    private Collider[] _hits =  new Collider[5];
    
    //TODO fix animations and advance melee combat
    protected override void Attack(Vector3 aimPosition, GameObject instigator, int team)
    {
        base.Attack(aimPosition, instigator, team);
        
        Vector3 origin = instigator.transform.position;
        Vector3 direction = (aimPosition - origin).normalized;

        int hitCount = Physics.OverlapSphereNonAlloc(origin, WeaponMeleeData.Range, _hits, WeaponMeleeData.AttackHitMask);

        for (var i = 0; i < hitCount; i++)
        {
            var hit = _hits[i];
            
            if (hit.gameObject == instigator) continue;

            //TODO decide how to manage friendly fire
            if (hit.TryGetComponent(out ITargetable target) && target.Team == team) continue;

            Vector3 targetDirection = (hit.transform.position - origin).normalized;
            float angleToHit = Vector3.Angle(direction, targetDirection);
            if (angleToHit > WeaponMeleeData.AttackAngle) continue;
            
            if (hit.TryGetComponent(out IParryUser parryUser) && parryUser.IsParrying)
            { 
                ParriedAttack(hit.gameObject, instigator);
                continue;
            }

            if (hit.TryGetComponent(out IDamageable damageable))
            {
                DamageInfo damageInfo = new DamageInfo(WeaponMeleeData.Damage, hit.gameObject, gameObject, instigator, WeaponMeleeData.DamageType);
                damageable.Damage(damageInfo);
            }
        }
    }

    public void ParriedAttack(GameObject victim, GameObject instigator)
    {
        //TODO add OnParrySuccessful event to start quick time event
        if (victim.TryGetComponent(out IDamageable damageable))
        {
            float parriedDamage = WeaponMeleeData.Damage - (WeaponMeleeData.Damage * ParryEfficiency);
            DamageInfo damageInfo = new DamageInfo(parriedDamage, victim, gameObject, instigator, WeaponMeleeData.DamageType);
            damageable.Damage(damageInfo);
        }
    }
    
    public bool TryParry(Vector3 aimPosition, GameObject instigator, int team)
    {
        float nextParryTime = _lastParryTime + (1/WeaponMeleeData.ParryRate);
        
        if (Time.time > nextParryTime)
        {
            Parry(aimPosition, instigator, team);
            _lastParryTime =  Time.time;
            return true;
        }
        
        return false;
    }

    private void Parry(Vector3 aimPosition, GameObject instigator, int team)
    {
        Vector3 origin = instigator.transform.position;
        Vector3 direction = (aimPosition - origin).normalized;
        int hitParriedCount = Physics.OverlapSphereNonAlloc(origin, WeaponMeleeData.Range, _hits, WeaponMeleeData.ParryHitMask);
        
        for (var i = 0; i < hitParriedCount; i++)
        {
            var hit = _hits[i];
            
            if (hit.gameObject == instigator) continue;
            
            if (hit.TryGetComponent(out ITargetable target) && target.Team == team) continue;
            
            Vector3 targetDirection = (hit.transform.position - origin).normalized;
            float angleToHit = Vector3.Angle(direction, targetDirection);
            if (angleToHit > WeaponMeleeData.AttackAngle) continue;
            
            if (hit.TryGetComponent(out IParryable parriedObject)) parriedObject.ParryEfficiency = WeaponMeleeData.ParryEfficiency;
            
        }
        
        if (instigator.TryGetComponent(out IParryUser parryUser))
        {
            parryUser.StartParryWindow(WeaponMeleeData.ParryAttackWindow);
        }
        
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, WeaponMeleeData.Range);
    }
    
}
