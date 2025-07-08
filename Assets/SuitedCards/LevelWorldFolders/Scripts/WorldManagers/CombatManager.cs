using UnityEngine;
using UnityEngine.InputSystem;

public class CombatManager : MonoBehaviour
{
    public static CombatManager Instance { get; private set; }

    public InputActionAsset InputActions;

    private InputAction a_endCombatAction;
    private EnemyManager enemy;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            //gameObject.SetActive(false);
        }
        else
        {
            Destroy(gameObject); // Ensure only one instance exists
        }
    }
    private void OnEnable()
    {
        InputActions.FindActionMap("WorldCombatTest").Enable();
    }
    private void OnDisable()
    {
        InputActions.FindActionMap("WorldCombatTest").Disable();
    }
    public void InitializeCombat(EnemyManager enemy)
    {
        AssignInputActions();
        this.enemy = enemy;
    }
    private void AssignInputActions()
    {
        a_endCombatAction = InputActions.FindAction("WorldCombatTest/EndCombat");
        if (a_endCombatAction != null)
        {
            a_endCombatAction.performed += ctx => EndCombat();
        }
    }
    public void EndCombat()
    {
        this.enemy.OnDefeated();

        // Logic to end combat mode
        Debug.Log("Combat ended. Switching back to roaming mode.");
        GameModeManager.Instance.SwitchGameMode(GameMode.Roaming);
        
        // Optionally, you can disable the combat manager or reset its state
        gameObject.SetActive(false);
    }
}
