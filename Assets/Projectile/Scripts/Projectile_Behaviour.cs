using System;
using UnityEngine;

public class Projectile_Behaviour : MonoBehaviour
{
    // Projectile Variables
    private Rigidbody2D _rigidbody;
    private Vector3 _direction;

    public Projectile_Data projectileData;


    // Homing Variables
    private GameObject _target;
    
    // Explosive Variables
    private float _currentExplosionTime;
    private float _spawnTime = 0.3f;
    
    // Player Variables
    private float _duration;
    

    private void Start()
    {
        _currentExplosionTime = projectileData.timeToExplode;
        _rigidbody = GetComponent<Rigidbody2D>();
        if (projectileData.lifeTime > 0)
        {
            Destroy(gameObject, _duration);
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
                    _direction = _target.transform.position - transform.position;
                    _direction.Normalize();
                    _rigidbody.velocity = _direction * projectileData.speed;
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
            projectile.GetComponent<Projectile_Behaviour>().projectileData = projectileData.childProjectile;
            projectile.GetComponent<Projectile_Behaviour>().SetDirection(direction);
        }
        Destroy(gameObject);
    }

    private void OnCollisionEnter2D(Collision2D col)
    {
        switch (col.gameObject.tag)
        {
            case "Enemy":
                // col.gameObject.GetComponent<Enemy_Behaviour>().TakeDamage(projectileData.damage);
                Debug.Log("Enemy Hit for" + projectileData.damage);
                Destroy(gameObject);
                break;
            case "Wall":
                Destroy(gameObject);
                break;
        }
        {
            
        }
    }

    public void SetDirection(Vector3 direction)
    {
        _direction = direction;
    }
    
    public void SetTarget(GameObject target)
    {
        _target = target;
    }
    
    public float Duration
    {
        get => _duration;
        set => _duration = value;
    }
}
