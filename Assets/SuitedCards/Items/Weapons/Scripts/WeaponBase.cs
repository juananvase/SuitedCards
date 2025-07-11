using UnityEngine;

public abstract class WeaponBase : ItemBase

{
    public WeaponData WeaponData => ItemData as WeaponData;

    protected virtual void HandleAttack(Vector3 aimPosition, GameObject instigator, int team, float weaponDamage, bool isParryable) { }

    public void Attack(Vector3 aimPosition, GameObject instigator, int team)
    {
        HandleAttack(aimPosition, instigator, team, WeaponData.Damage, true);
    }

    public void ChargedAttack(Vector3 aimPosition, GameObject instigator, int team)
    {
        float criticDamage = WeaponData.Damage + (WeaponData.Damage * WeaponData.CriticDamagePercentage);
        HandleAttack(aimPosition, instigator, team, criticDamage, true);
    }

    //TODO add animmation
    //TODO add SFX
}
