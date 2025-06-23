using UnityEngine;

public abstract class WeaponBase : MonoBehaviour
{
    [field: SerializeField] public WeaponData WeaponData { get; private set; }
    private float _lastAttackTime;
    
    public bool TryAttack(Vector3 aimPosition, GameObject instigator, int team)
    {
        float nextAttackTime = _lastAttackTime + (1f / WeaponData.AttackRate);
        
        if (Time.time > nextAttackTime)
        {
            Attack( aimPosition, instigator, team);
            _lastAttackTime =  Time.time;
            return true;
        }
        
        return false;
    }

    protected virtual void Attack(Vector3 aimPosition, GameObject instigator, int team)
    {
        
    }

    //TODO add animmation
    //TODO add SFX
}
