using UnityEngine;

public class CharacterManager : MonoBehaviour
{
    protected CharacterController characterController;

    protected virtual void Awake()
    {
        // Initialize character-related components or settings here

        characterController = GetComponent<CharacterController>();
        if (characterController == null) // ADD CHARACTER CONTROLLER IF THERE IS NONE ATTACHED TO THE GAME OBJECT
        {
            characterController = gameObject.AddComponent<CharacterController>();
        }
    }
    protected virtual void OnEnable()
    {
        // Enable character-related components or settings here
    }
    protected virtual void OnDisable()
    {
        // Disable character-related components or settings here
    }
    protected virtual void Update()
    {
        // Handle character updates here, such as movement, animations, etc.
        // This method can be overridden in derived classes for specific character behaviors
    }
    protected virtual void FixedUpdate()
    {
        // Handle physics-related updates here, such as applying forces or checking collisions
        // This method can be overridden in derived classes for specific character behaviors
    }
}
