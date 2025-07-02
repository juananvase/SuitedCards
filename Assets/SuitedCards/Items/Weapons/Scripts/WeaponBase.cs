using UnityEngine;

public abstract class WeaponBase : MonoBehaviour
{
    [field: SerializeField] public WeaponData WeaponData { get; private set; }
    private float _lastAttackTime;
    
    public bool TryAttack(Vector3 aimPosition, GameObject instigator, int team)
    {
        Attack( aimPosition, instigator, team);
        return true;
    }

    protected virtual void Attack(Vector3 aimPosition, GameObject instigator, int team) { }

    //TODO add animmation
    //TODO add SFX
}
