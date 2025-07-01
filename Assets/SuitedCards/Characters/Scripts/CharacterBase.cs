using System;
using System.Collections;
using PrimeTween;
using Unity.VisualScripting;
using UnityEngine;

[RequireComponent(typeof(CharacterHealth))]
public abstract class CharacterBase : MonoBehaviour, ITargetable, IParryUser
{
    [Header("Data")]
    [field: SerializeField] public CharacterData CharacterData { get; private set; }
    [field: SerializeField] public WeaponBase[] Weapons { get; private set; }
    [field: SerializeField] public Transform[] WeaponAnchors { get; private set; }
    
    [Header("Target System")]
    [SerializeField] protected GameObject _target;
    [field: SerializeField] public int Team { get; set; } = 0;
    [field: SerializeField] public bool IsTargetable { get; set; } =  true;
    
    [Header("Parry System")]
    [field: SerializeField] public bool IsParrying { get; set; } = false;
    private Coroutine _parryWindow =  null;
    
    private float _lastDashTime;
    private Tween _dashTweenAnimation;
    private Coroutine _attackRoutine;

    private void OnEnable()
    {
        CharacterData.OnFindWeapons.AddListener(TryFindWeapons);
    }
    
    private void OnDisable()
    {
        CharacterData.OnFindWeapons.RemoveListener(TryFindWeapons);
    }

    private void TryFindWeapons(GameObject character)
    {
        if(character == gameObject) FindWeapons();
    }

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
        if(_target.Equals(null)) return;
        Vector3 direction = (_target.transform.position - transform.position).normalized;
        Vector3 dashDistance = new Vector3(CharacterData.DashMultiplier * direction.x, CharacterData.DashYOffset, CharacterData.DashMultiplier * direction.z);
        
        _dashTweenAnimation = Tween.Position(transform,startValue: transform.position, endValue: dashDistance, duration: CharacterData.DashDuration, ease: Ease.InCubic, cycles: 2, cycleMode: CycleMode.Rewind);
        
    }
    
    [ContextMenu(nameof(Attack))]
    protected void Attack()
    {
        if(Weapons.Length == 0 || _target.Equals(null) || _attackRoutine != null) return;
        _attackRoutine = StartCoroutine(AttackRoutine());
    }
    
    protected IEnumerator AttackRoutine()
    {
        for (int i = 0; i < Weapons.Length; i++)
        {
            yield return new WaitForSeconds(0.3f);
            
            switch (Weapons[i])
            {
                case WeaponMelee:
                    yield return MeleeAttackRoutine((WeaponMelee)Weapons[i]);
                    continue;
                
                case WeaponRangeProjectile:
                    yield return RangeProjectileAttackRoutine((WeaponRangeProjectile)Weapons[i]);
                    continue;
            }
        }

        yield return _attackRoutine = null;
    }
    
    protected IEnumerator RangeProjectileAttackRoutine(WeaponRangeProjectile weaponRangeProjectile)
    {
        yield return weaponRangeProjectile.TryAttack(_target.transform.position, gameObject, Team);
    }
    
    protected IEnumerator MeleeAttackRoutine(WeaponMelee weaponMelee)
    {
        float targetDistance = Vector3.Distance(transform.position, _target.transform.position);
        Vector3 direction = (_target.transform.position - transform.position).normalized;
        
        float attackDistance = targetDistance - CharacterData.AttackOffset;
        Vector3 attackDirection = new Vector3(attackDistance * direction.x, CharacterData.DashYOffset, attackDistance * direction.z);;
        
        Vector3 startPosition = transform.position;
        
        yield return Tween.Position(transform, startValue: startPosition, endValue: attackDirection, duration: CharacterData.DashDuration, ease: Ease.InCubic, cycleMode: CycleMode.Rewind).ToYieldInstruction();
        yield return Tween.Delay(CharacterData.AttackDuration).ToYieldInstruction();
        yield return weaponMelee.TryAttack(_target.transform.position, gameObject, Team);
        yield return Tween.Position(transform, startValue: attackDirection, endValue: startPosition, duration: CharacterData.DashDuration, ease: Ease.InCubic, cycleMode: CycleMode.Rewind).ToYieldInstruction();
    }

    [ContextMenu(nameof(Parry))]
    protected void Parry()
    {
        if(Weapons.Length == 0 || _target.Equals(null)) return;
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
