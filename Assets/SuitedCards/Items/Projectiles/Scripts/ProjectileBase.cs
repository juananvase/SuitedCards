using System;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(Collider))]
public abstract class ProjectileBase : MonoBehaviour
{
    [SerializeField] private Rigidbody _rigidbody;
    private float _speed;
    private float _range;
    private float _damage;
    private DamageType _damageType;
    private GameObject _instigator;
    private int _team;
    private Vector3 _spawnPosition;

    private void OnValidate()
    {
        _rigidbody = GetComponent<Rigidbody>();
        _rigidbody.useGravity = false;
        _rigidbody.collisionDetectionMode = CollisionDetectionMode.ContinuousDynamic;
        _rigidbody.interpolation = RigidbodyInterpolation.Interpolate;
        
        Collider collider = GetComponent<Collider>();
        collider.isTrigger = true;
    }

    public void Launch(float speed, float range, float damage, DamageType damageType, GameObject instigator, int team)
    {
        _speed = speed;
        _range = range;
        _damage = damage;
        _damageType = damageType;
        _instigator = instigator;
        _team = team;
        
        _rigidbody.linearVelocity = transform.forward * _speed;
        _spawnPosition = transform.position;
    }

    private void Update()
    {
        float distanceTraveled = Vector3.Distance(transform.position, _spawnPosition);

        if (distanceTraveled > _range)
        {
            Cleanup();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        ProjectileBehavior(other);

    }
    
    protected virtual void ProjectileBehavior(Collider other)
    {
        if (other.TryGetComponent(out ITargetable target) && target.Team == _team) return;

        if (other.TryGetComponent(out IDamageable damageable))
        {
            DamageInfo damageInfo = new DamageInfo(_damage, other.gameObject, gameObject, _instigator, _damageType);
            damageable.Damage(damageInfo);
        }
        
        Cleanup();
    }

    protected void Cleanup()
    {
        Destroy(gameObject);
    }
}
