using UnityEngine;

public class EquipmentBase : ItemBase
{
    public EquipmentData EquipmentData => ItemData as EquipmentData;

    public void Equip(GameObject instigator)
    {
        ApplyShield(instigator);
    }

    private void ApplyShield(GameObject instigator)
    {
        if (!instigator.TryGetComponent(out IShieldable shielded)) return;
        
        shielded.Shield += EquipmentData.Shield;
    }
}
