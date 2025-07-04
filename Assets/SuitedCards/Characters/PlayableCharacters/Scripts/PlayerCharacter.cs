using System;
using System.Collections;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
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
        if(_qteAction.WasPressedThisFrame()) ReadQTEInputs(_qteAction.ReadValue<Vector2>());
        
        //Debug Input
        if (Input.GetKeyDown(KeyCode.A)) Attack();
    }
    
    protected override async Task RangeProjectileAttackRoutine(WeaponRangeProjectile weaponRangeProjectile)
    {
        if (await GameManager.instance.QTEManager.QTESequence(4, PlayerCharacterData.QTEReactionWindow))
        {
            weaponRangeProjectile.ChargedAttack(_target.transform.position, gameObject, Team);
            return;
        }
        
        weaponRangeProjectile.Attack(_target.transform.position, gameObject, Team);
        await Awaitable.NextFrameAsync();
    }
    
    protected override async Task MeleeAttackRoutine(WeaponMelee weaponMelee)
    {
        Vector3[] movementPositions = CalculateMovementPositions();

        await Tween.Position(transform, startValue: movementPositions[0], endValue: movementPositions[1], duration: CharacterData.DashDuration, ease: Ease.InCubic);
        await Tween.Delay(CharacterData.AttackDuration);
        
        if (await GameManager.instance.QTEManager.QTESequence(4, PlayerCharacterData.QTEReactionWindow))
        {
            weaponMelee.ChargedAttack(_target.transform.position, gameObject, Team);
            await Awaitable.NextFrameAsync();
            await Tween.Position(transform, startValue: movementPositions[1], endValue: movementPositions[0], duration: CharacterData.DashDuration, ease: Ease.InCubic, cycleMode: CycleMode.Rewind);
            return;
        }
        
        weaponMelee.Attack(_target.transform.position, gameObject, Team);
        await Awaitable.NextFrameAsync();
        await Tween.Position(transform, startValue: movementPositions[1], endValue: movementPositions[0], duration: CharacterData.DashDuration, ease: Ease.InCubic, cycleMode: CycleMode.Rewind);
    }

    private void ReadQTEInputs(Vector2 value)
    {
        GameManager.instance.QTEManager.ReadQTEInputs(value);
    }
    
    protected override async Task CounterAttackTask()
    {
        if (await GameManager.instance.QTEManager.QTESequence(3, PlayerCharacterData.QTEReactionWindow))
        {
            Attack();
            return;
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
