using System;
using System.Collections;
using System.Diagnostics;
using PrimeTween;
using UnityEngine;
using UnityEngine.InputSystem;
using Debug = UnityEngine.Debug;
using Random = UnityEngine.Random;

public class PlayerCharacter : CharacterBase
{
    //Data
    public PlayerCharacterData PlayerCharacterData => CharacterData as PlayerCharacterData;
    
    //Input System
    [SerializeField] private InputActionAsset _inputActions;
    
    private InputAction _selectAction;
    private InputAction _dashAction;
    private InputAction _parryAction;
    private InputAction _qteAction;
    
    //Target System
    [SerializeField] private LayerMask _targetMask;
    
    //QTE system
    [SerializeField] private Vector2[] _patternSequence;

    protected override void OnEnable()
    {
        base.OnEnable();
        _inputActions.FindActionMap("Combat").Enable();
    }

    protected override void OnDisable()
    {
        base.OnDisable();
        _inputActions.FindActionMap("Combat").Disable();
    }

    private void Awake()
    {
        _selectAction = _inputActions.FindAction("Select");
        _dashAction = _inputActions.FindAction("Dash");
        _parryAction = _inputActions.FindAction("Parry");
        _qteAction = _inputActions.FindAction("QTE");
    }

    private void Update()
    {
        if(_selectAction.WasPressedThisFrame()) SelectTarget();
        if(_dashAction.WasPressedThisFrame()) TryDash();
        if(_parryAction.WasPressedThisFrame()) Parry();
        if(_qteAction.WasPressedThisFrame()) ReadQTEInputs(_qteAction.ReadValue<Vector2>(), 4);
        
        //Debug Input
        if (Input.GetKeyDown(KeyCode.A)) Attack();
    }

    protected override IEnumerator RangeProjectileAttackRoutine(WeaponRangeProjectile weaponRangeProjectile)
    {
        yield return weaponRangeProjectile.TryAttack(_target.transform.position, gameObject, Team);
    }
    
    protected override IEnumerator MeleeAttackRoutine(WeaponMelee weaponMelee)
    {
        float targetDistance = Vector3.Distance(transform.position, _target.transform.position);
        Vector3 direction = (_target.transform.position - transform.position).normalized;
        
        float attackDistance = targetDistance - CharacterData.AttackOffset;
        Vector3 attackDirection = new Vector3(attackDistance * direction.x, CharacterData.DashYOffset, attackDistance * direction.z);;
        
        Vector3 startPosition = transform.position;
        
        yield return Tween.Position(transform, startValue: startPosition, endValue: attackDirection, duration: CharacterData.DashDuration, ease: Ease.InCubic, cycleMode: CycleMode.Rewind).ToYieldInstruction();
        yield return Tween.Delay(CharacterData.AttackDuration).ToYieldInstruction();
        GameManager.instance.TimeManager.DoSlowMotion(PlayerCharacterData.ReactionWindow);
        yield return weaponMelee.TryAttack(_target.transform.position, gameObject, Team);
        yield return Tween.Position(transform, startValue: attackDirection, endValue: startPosition, duration: CharacterData.DashDuration, ease: Ease.InCubic, cycleMode: CycleMode.Rewind).ToYieldInstruction();
    }
    
    private void ReadQTEInputs(Vector2 value, int patternNumber)
    {
        for (int i = 0; i < patternNumber; i++)
        {
            _patternSequence[i] = new Vector2(Random.Range(-1, 1), Random.Range(-1, 1));
        }
    }

    private void SelectTarget()
    {
        Ray mouseRay = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(mouseRay, out RaycastHit hitInfo, Mathf.Infinity, _targetMask))
        {
            if (hitInfo.collider.gameObject != gameObject) _target = hitInfo.collider.gameObject;
        }

    }
}
