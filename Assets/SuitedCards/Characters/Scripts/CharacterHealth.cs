using System;
using UnityEngine;

public class CharacterHealth : HealthBase, IShieldable
{
    protected CharacterData _characterData => _data as CharacterData;
    
    public float Shield { get; set; }

    public override void Damage(DamageInfo damageInfo)
    {
        if (!MeetDamageConditions(damageInfo)) return;
        
        float damage = Defend(damageInfo.Amount);
        
        _currentHealth -= damage;
        _currentHealth = Mathf.Clamp(_currentHealth, 0f, _characterData.MaxHealth);
        
        OnDamage?.Invoke(damageInfo);
        
        //TODO add damage feedback
        if (!IsAlive)
        {
            OnDeath?.Invoke(damageInfo);
        }
    }

    public float Defend(float damageTaken)
    {
        Shield -= damageTaken;
        Shield = Mathf.Clamp(Shield, 0f, 999);
        
        damageTaken -= Shield;
        damageTaken = Mathf.Clamp(damageTaken, 0f, 999);
        return damageTaken;
    }
}
