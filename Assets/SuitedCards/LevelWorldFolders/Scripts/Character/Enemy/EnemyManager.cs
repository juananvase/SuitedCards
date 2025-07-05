using UnityEngine;
using UnityEngine.Events;

public class EnemyManager : MonoBehaviour, IAttackable
{
    public UnityEvent OnAttackPerformed; // Event to trigger when the enemy is attacked
    public void OnAttacked()
    {
        Debug.Log("Enemy has been attacked!");
        OnAttackPerformed.Invoke(); // Invoke the event when the enemy is attacked
    }
}
