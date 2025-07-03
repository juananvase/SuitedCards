using UnityEngine;

public abstract class WeaponBase : MonoBehaviour
{
    [field: SerializeField] public WeaponData WeaponData { get; private set; }

    public virtual void Attack(Vector3 aimPosition, GameObject instigator, int team) { }
    public virtual void ChargedAttack(Vector3 aimPosition, GameObject instigator, int team) { }

    //TODO add animmation
    //TODO add SFX
}
