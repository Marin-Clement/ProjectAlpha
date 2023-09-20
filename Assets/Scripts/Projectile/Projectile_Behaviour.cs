using System;
using System.Collections.Generic;
using UnityEngine;

public class Projectile_Behaviour : MonoBehaviour
{
    // Projectile Variables
    private Rigidbody2D _rigidbody;
    private CapsuleCollider2D _collider;
    private Vector3 _direction;
    public Projectile_Data projectileData;

    // Homing Variables
    private GameObject _target;
    
    // Explosive Variables
    private float _currentExplosionTime;
    private float _spawnTime = 0.2f;
    
    // Player Variables
    private float _duration;
    
    // Pierce Variables
    private int _pierceCount;
    private int _entityPierced;
    
    // Bouncy Variables
    private int _bounceCount;

    // Critical Variables
    private bool _isCritical;

    // Damage Variables

    private List<object> _damage;

    private void Start()
    {
        transform.up = _direction;
        // Declare Variables
        _currentExplosionTime = projectileData.timeToExplode;
        _pierceCount += projectileData.pierceCount;
        _bounceCount += projectileData.bounces;
        
        _rigidbody = GetComponent<Rigidbody2D>();
        _collider = GetComponent<CapsuleCollider2D>();
        
        switch (gameObject.tag)
        {
            case "PlayerProjectile":
                Destroy(gameObject, _duration);
                break;
            case "EnemyProjectile":
                Destroy(gameObject, projectileData.lifeTime);
                break;
        }

        if (projectileData.isHoming)
        {
            _target = GameObject.FindGameObjectWithTag("Enemy");
        }
    }

    private void Update()
    {
        if (projectileData.isHoming)
        {
            if (_target)
            {
                _spawnTime -= Time.deltaTime;
                if (_spawnTime <= 0)
                {
                    Vector3 targetPosition = _target.transform.position;
                    Vector3 desiredDirection = (targetPosition - transform.position).normalized;

                    Vector3 newDirection = Vector3.Lerp(_rigidbody.velocity.normalized, desiredDirection, projectileData.turnSpeed * Time.deltaTime);
                    _rigidbody.velocity = newDirection * projectileData.speed;
                }
                else
                {
                    _rigidbody.velocity = _direction * projectileData.speed;
                }
            }
            else
            {
                _rigidbody.velocity = _direction * projectileData.speed;
            }
        }
        else
        {
            _rigidbody.velocity = _direction * projectileData.speed;
        }
        if (!projectileData.isExplosive) return;
        _currentExplosionTime -= Time.deltaTime;
        if (_currentExplosionTime <= 0)
        {
            Explode();
            _currentExplosionTime = projectileData.timeToExplode;
        }
    }
    
    private void Explode()
    {
        int angle = 360 / projectileData.numberOfProjectiles;
        for (int i = 0; i < projectileData.numberOfProjectiles; i++)
        {
            Vector3 direction = Quaternion.AngleAxis(angle * i, Vector3.forward) * transform.up;
            GameObject projectile = Instantiate(gameObject, transform.position, Quaternion.identity);
            Projectile_Behaviour projectileBehaviour = projectile.GetComponent<Projectile_Behaviour>();
            projectileBehaviour.SetDirection(direction);
            projectileBehaviour.Damage = Damage;
            projectileBehaviour.Damage[0] = (float) projectileData.childProjectile.damage;
            projectileBehaviour.gameObject.transform.up = direction;
            projectileBehaviour.projectileData = projectileData.childProjectile;
            projectileBehaviour.Duration = projectileData.lifeTime;
        }
        Destroy(gameObject);
    }

    private void OnCollisionEnter2D(Collision2D col)
    {
        switch (col.gameObject.tag)
        {
            case "Player":
                if (gameObject.CompareTag("EnemyProjectile"))
                {
                    col.gameObject.GetComponent<Health>().TakeDamage(_damage);
                    Destroy(gameObject);
                }
                break;
            case "Wall":
                if (projectileData.isBouncy)
                {
                    if (_bounceCount > 0)
                    {
                        _bounceCount--;
                        _direction = Vector3.Reflect(_direction, col.contacts[0].normal);
                        transform.up = _direction;
                    }
                    else
                    {
                        Destroy(gameObject);
                    }
                }
                else
                {
                    Destroy(gameObject);
                }
                break;
        }
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        switch (col.gameObject.tag)
        {
            case "Enemy":
                if (col.gameObject.CompareTag("Enemy"))
                {
                    if (_pierceCount > 0)
                    {
                        _pierceCount--;
                        _entityPierced++;
                        Damage[0] = (float) Damage[0] - _entityPierced * (float) Damage[0] * 0.2f;
                    }
                    else
                    {
                        Destroy(gameObject);
                    }
                }
                break;
        }
    }

    public void SetDirection(Vector3 direction)
    {
        _direction = direction;
    }

    private void OnDestroy()
    {
        Instantiate(projectileData.deathParticle, transform.position, Quaternion.identity);
    }

    public float Duration
    {
        get => _duration;
        set => _duration = value;
    }

    public List<object> Damage
    {
        get => _damage;
        set => _damage = value;
    }

    public Boolean IsCritical
    {
        get => _isCritical;
        set => _isCritical = value;
    }
}
