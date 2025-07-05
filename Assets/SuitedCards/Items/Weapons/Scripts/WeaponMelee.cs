using System;
using UnityEngine;

public class WeaponMelee : WeaponBase, IParryable
{
    public WeaponMeleeData WeaponMeleeData => WeaponData as WeaponMeleeData;
    public float ParryEfficiency { get; set; }

    private float _lastParryTime;
    private Collider[] _hits =  new Collider[5];

    //TODO fix animations and advance melee combat
    protected override void HandleAttack(Vector3 aimPosition, GameObject instigator, int team, float damage, bool isParryable)
    {
        base.HandleAttack(aimPosition, instigator, team, damage, isParryable);
        
        Vector3 origin = instigator.transform.position;
        Vector3 direction = (aimPosition - origin).normalized;

        int hitCount = Physics.OverlapSphereNonAlloc(origin, WeaponMeleeData.Range, _hits, WeaponMeleeData.AttackHitMask);

        for (int i = 0; i < hitCount; i++)
        {
            Collider hit = _hits[i];
            
            if (hit.gameObject == instigator) continue;

            //TODO decide how to manage friendly fire
            if (hit.TryGetComponent(out ITargetable target) && target.Team == team) continue;

            Vector3 targetDirection = (hit.transform.position - origin).normalized;
            float angleToHit = Vector3.Angle(direction, targetDirection);
            if (angleToHit > WeaponMeleeData.AttackAngle) continue;
            
            if (isParryable && hit.TryGetComponent(out IParryUser parryUser) && parryUser.IsParrying)
            { 
                ParriedAttack(hit.gameObject, instigator, WeaponMeleeData.Damage);
                return;
            }

            if (hit.TryGetComponent(out IDamageable damageable))
            {
                DamageInfo damageInfo = new DamageInfo(damage, hit.gameObject, gameObject, instigator, WeaponMeleeData.DamageType);
                damageable.Damage(damageInfo);
            }
        }
        
    }

    public virtual void CounterAttack(Vector3 aimPosition, GameObject instigator, int team)
    {
        float counterDamage = WeaponData.Damage * WeaponMeleeData.ParryEfficiency;
        HandleAttack(aimPosition, instigator, team, counterDamage, false);
    }

    public void ParriedAttack(GameObject victim, GameObject instigator, float baseDamage)
    {
        float parriedDamage = baseDamage - (baseDamage * ParryEfficiency);
        WeaponMeleeData.OnParrySuccessful?.Invoke(victim);
        if (victim.TryGetComponent(out IDamageable damageable))
        {
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
        
        //TODO change to event?
        if (instigator.TryGetComponent(out IParryUser parryUser))
        {
            parryUser.StartParryWindow(WeaponMeleeData.ParryOpportunityWindow);
        }
        
    }
    
}
