using UnityEngine;

public class EnemyManager : MonoBehaviour, IAttackable
{
    public void OnAttacked()
    {
        Debug.Log("Enemy has been attacked!");
    }
}
