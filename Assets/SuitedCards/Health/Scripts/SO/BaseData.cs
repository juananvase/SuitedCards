using UnityEngine;

[CreateAssetMenu(fileName = "BaseData", menuName = "Scriptable Objects/BaseData")]
public class BaseData : ScriptableObject
{
    [Header("Health")]
    [field: SerializeField] public float MaxHealth { get; private set; } = 100f;
}
