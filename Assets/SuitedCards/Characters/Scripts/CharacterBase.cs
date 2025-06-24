using System;
using UnityEngine;

[RequireComponent(typeof(Health))]
public abstract class CharacterBase : MonoBehaviour, ITargetable
{
    [field: SerializeField] public CharacterData CharacterData { get; private set; }
    [field: SerializeField] public WeaponBase[] Weapons { get; private set; }
    
    [field: SerializeField] public int Team { get; set; } = 0;
    [field: SerializeField] public bool IsTargetable { get; set; } =  true;
    
    [Header("Test (Temporal variables)")]
    [SerializeField] private GameObject _target;
    
    private float _lastDashTime;
    private float _lastParryTime;
    
    [ContextMenu(nameof(FindWeapons))]
    private void FindWeapons()
    {
        Weapons = GetComponentsInChildren<WeaponBase>();
    }

    public bool TryDash(Vector3 dashDirection)
    {
        if (dashDirection == Vector3.zero)
        {
            Debug.LogError("The dash direction must be greater than cero");
            return false;
        }

        float nextDashTime = _lastDashTime + (1/CharacterData.DashRate);
        
        if (Time.time > nextDashTime)
        {
            Dash(dashDirection);
            return true;
        }
        
        return false;
    }

    private void Dash(Vector3 dashDirection)
    {
        
    }

    //TODO implement parry on melee weapons instead (Design desition to make)
    public bool TryParry()
    {
        float nextParryTime = _lastParryTime + (1/CharacterData.ParryRate);
        
        if (Time.time > nextParryTime)
        {
            Parry();
            return true;
        }
        
        return false;
    }

    private void Parry()
    {
        
    }
    
    [ContextMenu(nameof(Attack))]
    private void Attack()
    {
        WeaponBase weapon = Weapons[0];
        weapon.TryAttack(_target.transform.position, gameObject, Team);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Attack();
        }
    }
}
