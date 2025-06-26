using System;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Events;

public class HealthBase : MonoBehaviour, IDamageable, IHealable
{
    [SerializeField] private BaseData _data;
    [SerializeField] private float _currentHealth = 100f;
    
    public float HealthPercentage => _currentHealth / _data.MaxHealth;
    public bool IsAlive => _currentHealth >= 1;
    
    public UnityEvent<HealingInfo> OnHeal;
    public UnityEvent<DamageInfo> OnDamage;
    public UnityEvent<DamageInfo> OnDeath;

    public void Damage(DamageInfo damageInfo)
    {
        if(!MeetDamageConditions(damageInfo)) return;

        _currentHealth -= damageInfo.Amount;
        _currentHealth = Mathf.Clamp(_currentHealth, 0f, _data.MaxHealth);
        
        OnDamage?.Invoke(damageInfo);
        
        //TODO add damage feedback
        if (!IsAlive)
        {
            OnDeath?.Invoke(damageInfo);
        }
    }

    protected virtual bool MeetDamageConditions(DamageInfo damageInfo)
    {
        if (!IsAlive)
        {
            Debug.LogWarning("Trying to deal damage to a death entity");
            return false;
        }
        
        if(damageInfo == null)
        {
            Debug.LogError("Damage Info is null");
            return false;
        }
        
        if(damageInfo.Amount < 0f)
        {
            Debug.LogError("Trying to deal negative damage");
            return false;
        }
        
        return true;
    }
    
    public void Heal(HealingInfo healingInfo)
    {
        if(!MeetHealingConditions(healingInfo)) return;

        _currentHealth += healingInfo.Amount;
        _currentHealth = Mathf.Clamp(_currentHealth, 0f, _data.MaxHealth);
        
        OnHeal?.Invoke(healingInfo);
        //TODO add healing feedback
    }

    protected virtual bool MeetHealingConditions(HealingInfo healingInfo)
    {
        if (!IsAlive)
        {
            Debug.LogWarning("Trying to heal a death entity");
            return false;
        }
        
        if(healingInfo == null)
        {
            Debug.LogError("Healing Info is null");
            return false;
        }
        
        if(healingInfo.Amount < 1f)
        {
            Debug.LogError("Trying to heal less than 1 health unit");
            return false;
        }
        
        return true;
    }
    
    [ContextMenu(nameof(DamageTest))]
    private void DamageTest()
    {
        DamageInfo damageInfo = new DamageInfo(10f, gameObject, gameObject, gameObject, DamageType.None);
        Damage(damageInfo);
    }
    
    [ContextMenu(nameof(HealingTest))]
    private void HealingTest()
    {
        HealingInfo healingInfo = new HealingInfo(10f, gameObject, gameObject, gameObject, HealingType.None);
        Heal(healingInfo);
    }
}

public class DamageInfo
{
    public DamageInfo(float amount, GameObject victim, GameObject source, GameObject instigator, DamageType damageType)
    {
        Amount = amount;
        Victim = victim;
        Source = source;
        Instigator = instigator;
        DamageType = damageType;
    }

    public float Amount { get; set; }
    public GameObject Victim { get; set; }
    public GameObject Source { get; set; }
    public GameObject Instigator { get; set; }
    public DamageType DamageType { get; set; }
}

public enum DamageType
{
    None,
    Physical
}

public class HealingInfo
{
    public HealingInfo(float amount, GameObject receiver, GameObject source, GameObject provider, HealingType healingType)
    {
        Amount = amount;
        Receiver = receiver;
        Source = source;
        Provider = provider;
        HealingType = healingType;
    }

    public float Amount { get; set; }
    public GameObject Receiver { get; set; }
    public GameObject Source { get; set; }
    public GameObject Provider { get; set; }
    public HealingType HealingType { get; set; }
}

public enum HealingType
{
    None,
    Instant,
    Timed
}