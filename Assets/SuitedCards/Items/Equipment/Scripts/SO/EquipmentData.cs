using UnityEngine;

[CreateAssetMenu(fileName = "EquipmentData", menuName = "Scriptable Objects/EquipmentData")]
public class EquipmentData : ItemData
{
    [field: SerializeField] public float Shield { get; private set; }
}
