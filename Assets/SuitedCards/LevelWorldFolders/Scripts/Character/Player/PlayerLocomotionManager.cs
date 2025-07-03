using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerLocomotionManager : CharacterLocomotionManager
{
    public InputActionAsset InputActions;

    private InputAction a_moveAction;

    private Vector2 movementVector;

    [SerializeField] private float moveSpeed = 5f;

    private Camera playerCamera;

    protected override void Awake()
    {
        base.Awake();
        playerCamera = Camera.main;
    }

    public void AssignInputActions()
    {
        a_moveAction = InputActions.FindAction("LevelWorldActions/Move");
    }

    public void HandleAllMovementInput()
    {
        // Handle all movement input functions here
        HandleVerticalHorizontalInputs();
    }

    public void HandleAllMovement()
    {
        // Move relative to camera
        if (playerCamera != null)
        {
            Vector3 camForward = playerCamera.transform.forward;
            Vector3 camRight = playerCamera.transform.right;

            // Project forward and right vectors onto the XZ plane
            camForward.y = 0f;
            camRight.y = 0f;
            camForward.Normalize();
            camRight.Normalize();

            Vector3 move = camForward * movementVector.y + camRight * movementVector.x;

            // Only rotate if there is movement input
            if (move.sqrMagnitude > 0.001f)
            {
                // Smoothly rotate player to face movement direction
                Quaternion targetRotation = Quaternion.LookRotation(move.normalized);
                transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, 10f * Time.deltaTime);
            }

            characterController.Move(move * moveSpeed * Time.deltaTime);
        }
    }

    private void HandleVerticalHorizontalInputs()
    {
        movementVector = a_moveAction.ReadValue<Vector2>();
    }
}
