using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerManager : CharacterManager
{
    public InputActionAsset InputActions;
    private PlayerLocomotionManager playerLocomotionManager;
    private PlayerActionManager playerActionManager;

    protected override void Awake()
        {
        base.Awake();

        // Initialize PlayerLocomotionManager
        playerLocomotionManager = GetComponent<PlayerLocomotionManager>();
        playerActionManager = GetComponent<PlayerActionManager>();

        // Initiialize PlayerLocomotionManager with CharacterController and InputActions
        playerLocomotionManager.Initialize(characterController, InputActions);
        playerActionManager.Initialize(InputActions);
    }
    protected override void OnEnable()
    {
        InputActions.FindActionMap("LevelWorldActions").Enable();
    }
    protected override void OnDisable()
    {
        InputActions.FindActionMap("LevelWorldActions").Disable();
    }
    protected override void Update()
    {
        base.Update();

        if (playerLocomotionManager == null) return;
        playerLocomotionManager.HandleAllMovementInput(); // Call the method to handle all movement functions
    }
    protected override void FixedUpdate()
    {
        base.FixedUpdate();

        if (playerLocomotionManager == null) return;
        playerLocomotionManager.HandleAllMovement(); // Call the method to handle all movement input functions
        // Handle player-specific physics-related updates here, such as applying forces or checking collisions
        // This method can be overridden in derived classes for specific player behaviors

    }
}
