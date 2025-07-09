using System;
using System.Collections;
using System.Threading.Tasks;
using PrimeTween;
using UnityEngine;

[RequireComponent(typeof(CharacterHealth))]
public abstract class CharacterBase : MonoBehaviour, ITargetable, IParryUser
{
    //Data
    [field: SerializeField] public CharacterData CharacterData { get; private set; }
    [field: SerializeField] public WeaponBase[] Weapons { get; private set; }
    [field: SerializeField] public Transform[] WeaponAnchors { get; private set; }
    
    [field: SerializeField] public EquipmentBase[] Equipments { get; private set; }
    [field: SerializeField] public Transform[] EquipmentsAnchors { get; private set; }
    
    //Target System
    [SerializeField] protected GameObject _target;
    [field: SerializeField] public int Team { get; set; } = 0;
    [field: SerializeField] public bool IsTargetable { get; set; } =  true;
    
    //Parry System
    [field: SerializeField] public bool IsParrying { get; set; } = false;
    private Coroutine _parryWindow =  null;
    
    //Dash System
    private float _lastDashTime;
    private Tween _dashTweenAnimation;
    
    //Attack system
    private bool _isAttacking;

    protected virtual void OnEnable()
    {
        CharacterData.OnFindWeapons.AddListener(TryFindWeapons);
        CharacterData.OnParrySuccessful.AddListener(TryCounterAttack);
    }
    
    protected virtual void OnDisable()
    {
        CharacterData.OnFindWeapons.RemoveListener(TryFindWeapons);
        CharacterData.OnParrySuccessful.RemoveListener(TryCounterAttack);
    }

    private void Start()
    {
        UseEquipment();
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
    
    protected async void Attack()
    {
        if(Weapons.Length == 0 || _target.Equals(null) || _dashTweenAnimation.isAlive || _isAttacking) return;
        
        _isAttacking = true;
        
        for (int i = 0; i < Weapons.Length; i++)
        {
            await Awaitable.WaitForSecondsAsync(0.3f);
            
            switch (Weapons[i])
            {
                case WeaponMelee:
                    await MeleeAttackRoutine((WeaponMelee)Weapons[i]);
                    continue;
                
                case WeaponRangeProjectile:
                    await RangeProjectileAttackRoutine((WeaponRangeProjectile)Weapons[i]);
                    continue;
            }
        }

        _isAttacking = false;
    }
    
    protected virtual async Task RangeProjectileAttackRoutine(WeaponRangeProjectile weaponRangeProjectile)
    {
        weaponRangeProjectile.Attack(_target.transform.position, gameObject, Team);
        await Awaitable.NextFrameAsync();
    }
    
    protected virtual async Task MeleeAttackRoutine(WeaponMelee weaponMelee)
    {
        Vector3[] movementPositions = CalculateMovementPositions();

        await Tween.Position(transform, startValue: movementPositions[0], endValue: movementPositions[1], duration: CharacterData.DashDuration, ease: Ease.InCubic);
        
        await Tween.Delay(CharacterData.AttackDuration);
        weaponMelee.Attack(_target.transform.position, gameObject, Team);
        await Awaitable.NextFrameAsync();
        
        await Tween.Position(transform, startValue: movementPositions[1], endValue: movementPositions[0], duration: CharacterData.DashDuration, ease: Ease.InCubic);
    }
    
    protected Vector3[] CalculateMovementPositions()
    {
        Vector3[] movementPositions = new Vector3[2];
        
        float targetDistance = Vector3.Distance(transform.position, _target.transform.position);
        Vector3 direction = (_target.transform.position - transform.position).normalized;
        float attackDistance = targetDistance - CharacterData.AttackOffset;
        
        Vector3 attackDirection = new Vector3(attackDistance * direction.x, CharacterData.DashYOffset, attackDistance * direction.z);
        Vector3 startPosition = transform.position;
        
        movementPositions[0] = startPosition;
        movementPositions[1] = attackDirection;
        
        return movementPositions;
    }

    private void TryCounterAttack(GameObject value)
    {
        if(value != gameObject) return;
        CounterAttackTask();
    }

    protected virtual async void CounterAttackTask()
    {
        WeaponMelee weaponParrier = null;
        for (int i = 0; i < Weapons.Length; i++)
        {
            if (Weapons[i].TryGetComponent(out WeaponMelee weaponMelee))
            {
                weaponParrier = weaponMelee;
                break;
            }
        }
        
        Vector3[] movementPositions = CalculateMovementPositions();

        await Tween.Position(transform, startValue: movementPositions[0], endValue: movementPositions[1], duration: CharacterData.DashDuration, ease: Ease.InCubic);
        
        await Tween.Delay(CharacterData.AttackDuration);
        weaponParrier.CounterAttack(_target.transform.position, gameObject, Team);
        await Awaitable.NextFrameAsync();
        
        await Tween.Position(transform, startValue: movementPositions[1], endValue: movementPositions[0], duration: CharacterData.DashDuration, ease: Ease.InCubic);
    }
    
    
    private void TryFindEquipments(GameObject character)
    {
        if(character == gameObject) FindEquipments();
    }
    
    [ContextMenu(nameof(FindEquipments))]
    private void FindEquipments()
    {
        Equipments = GetComponentsInChildren<EquipmentBase>();
    }

    private void UseEquipment()
    {
        if(Equipments.Length == 0) return;

        for (int i = 0; i < Equipments.Length; i++)
        {
            Equipments[i].Equip(gameObject);
        }

    }

    
    protected void Parry()
    {
        if(Weapons.Length == 0 || _target.Equals(null) || _dashTweenAnimation.isAlive) return;
        
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
    
    
    protected bool TryDash()
    {
        float nextDashTime = _lastDashTime + (1/CharacterData.DashRate);
        
        if (Time.time > nextDashTime && !_dashTweenAnimation.isAlive && !_target.Equals(null) && !_isAttacking)
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
}
