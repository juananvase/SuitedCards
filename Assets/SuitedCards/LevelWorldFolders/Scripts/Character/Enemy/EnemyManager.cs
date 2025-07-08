using UnityEngine;
using UnityEngine.Events;

public class EnemyManager : MonoBehaviour, IAttackable, ICombatable
{
    public UnityEvent OnAttackPerformed; // Event to trigger when the enemy is attacked
    public UnityEvent OnEnemyDefeated;
    public void OnAttacked()
    {
        Debug.Log("Enemy has been attacked!");
        OnCombatProvoked(); // Switch to combat mode when attacked
        OnAttackPerformed.Invoke(); // Invoke the event when the enemy is attacked
    }
    public void OnCombatProvoked()
    {
        GameModeManager.Instance.SwitchGameMode(GameMode.Combat);
        CombatManager.Instance.InitializeCombat(this);
    }

    public void OnDefeated()
    {
        OnEnemyDefeated.Invoke();
    }
}