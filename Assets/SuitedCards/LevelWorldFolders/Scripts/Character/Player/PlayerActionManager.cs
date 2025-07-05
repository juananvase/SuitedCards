using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerActionManager : CharacterActionManager
{
    private InputActionAsset InputActions;

    private InputAction a_attackAction;

    protected override void Awake()
    {
        base.Awake();
    }

    public void Initialize(InputActionAsset input)
    {
        this.InputActions = input;
        AssignInputActions();
    }

    public void AssignInputActions()
    {
        a_attackAction = InputActions.FindAction("LevelWorldActions/Attack");
        if (a_attackAction != null)
        {
            a_attackAction.performed += ctx => TryToPerformAttack();
        }
    }

    private void TryToPerformAttack()
    {
        // Add bool to check if the player can attack (e.g., cooldown, state checks)
        PerformAttack();
    }

    private void PerformAttack()
    {
        RaycastHit[] hits = Physics.SphereCastAll(transform.position + Vector3.up, 1f, transform.forward, 3f);
        foreach (RaycastHit hit in hits)
        {
            IAttackable attackable = hit.collider.GetComponent<IAttackable>();
            if (attackable != null)
            {
                attackable.OnAttacked();
            }
        }
    }
}
