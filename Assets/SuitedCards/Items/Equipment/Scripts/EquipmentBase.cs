using UnityEngine;

public class EquipmentBase : MonoBehaviour
{
    [SerializeField] private float _shield = 10;

    public void Equip(GameObject instigator)
    {
        ApplyShield(instigator);
    }

    private void ApplyShield(GameObject instigator)
    {
        if (!instigator.TryGetComponent(out IShieldable shielded)) return;
        
        shielded.Shield += _shield;
    }
}
