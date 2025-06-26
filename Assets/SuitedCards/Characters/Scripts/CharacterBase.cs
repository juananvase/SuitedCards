using System;
using System.Collections;
using PrimeTween;
using Unity.VisualScripting;
using UnityEngine;

[RequireComponent(typeof(CharacterHealth))]
public abstract class CharacterBase : MonoBehaviour, ITargetable, IParryUser
{
    [field: SerializeField] public CharacterData CharacterData { get; private set; }
    [field: SerializeField] public WeaponBase[] Weapons { get; private set; }
    
    [field: SerializeField] public int Team { get; set; } = 0;
    [field: SerializeField] public bool IsTargetable { get; set; } =  true;
    
    [field: SerializeField] public bool IsParrying { get; set; } = false;
    private Coroutine _parryWindow =  null;
    
    [Header("Test (Temporal variables)")]
    [SerializeField] private GameObject _target;
    
    private float _lastDashTime;
    private Tween _dashTweenAnimation;
    
    [ContextMenu(nameof(FindWeapons))]
    private void FindWeapons()
    {
        Weapons = GetComponentsInChildren<WeaponBase>();
    }
    
    [ContextMenu(nameof(TryDash))]
    protected bool TryDash()
    {
        float nextDashTime = _lastDashTime + (1/CharacterData.DashRate);
        
        if (Time.time > nextDashTime && !_dashTweenAnimation.isAlive)
        {
            Dash();
            _lastDashTime =  Time.time;
            return true;
        }
        
        return false;
    }
    
    private void Dash()
    {
        Vector3 direction = (_target.transform.position - transform.position).normalized;
        Vector3 dashDistance = new Vector3(CharacterData.DashMultiplier * direction.x, CharacterData.DashYOffset, CharacterData.DashMultiplier * direction.z);
        
        _dashTweenAnimation = Tween.Position(transform,startValue: transform.position, endValue: dashDistance, duration: CharacterData.DashDuration, ease: Ease.InCubic, cycles: 2, cycleMode: CycleMode.Rewind);
        
    }
    
    [ContextMenu(nameof(Attack))]
    protected void Attack()
    {
        WeaponBase weapon = Weapons[0];
        weapon.TryAttack(_target.transform.position, gameObject, Team);
    }
    
    [ContextMenu(nameof(Parry))]
    protected void Parry()
    {
        WeaponBase weapon = Weapons[0];
        if (weapon.TryGetComponent(out WeaponMelee weaponMelee))
        {
            weaponMelee.TryParry(_target.transform.position, gameObject, Team);
        }
    }

    public void StartParryWindow(float parryWindow)
    {
        if(_parryWindow != null) return;
        
        IsParrying = true;
        _parryWindow = StartCoroutine(ParryWindowRoutine(parryWindow));
    }

    private IEnumerator ParryWindowRoutine(float parryWindow)
    {
        yield return new WaitForSeconds(parryWindow);
        IsParrying = false;
        _parryWindow = null;
    }
}
