using UnityEngine;

public class Projectile_Behaviour : MonoBehaviour
{
    // Projectile Variables
    private Rigidbody2D _rigidbody;
    private BoxCollider2D _collider;
    private Vector3 _direction;
    public Projectile_Data projectileData;

    // Homing Variables
    private GameObject _target;
    
    // Explosive Variables
    private float _currentExplosionTime;
    private float _spawnTime = 0.3f;
    
    // Player Variables
    private float _duration;
    
    // Pierce Variables
    private int _pierceCount;
    private int _enemyPierced;
    
    // Bouncy Variables
    private int _bounceCount;

    private void Start()
    {
        // Declare Variables
        _currentExplosionTime = projectileData.timeToExplode;
        _pierceCount += projectileData.pierceCount;
        _bounceCount += projectileData.bounces;
        
        _rigidbody = GetComponent<Rigidbody2D>();
        _collider = GetComponent<BoxCollider2D>();
        
        switch (gameObject.tag)
        {
            case "PlayerProjectile":
                Destroy(gameObject, _duration);
                break;
            case "EnemyProjectile":
                Destroy(gameObject, projectileData.lifeTime);
                break;
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
            Projectile_Behaviour projectileBehaviour = projectile.GetComponent<Projectile_Behaviour>();
            projectileBehaviour.SetDirection(direction);
            projectileBehaviour.gameObject.transform.up = direction;
            projectileBehaviour.projectileData = projectileData.childProjectile;
            projectileBehaviour.Duration = projectileData.lifeTime;
        }
        Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        switch (col.tag)
        {
            case "Enemy":
                if (col.gameObject.CompareTag("Enemy"))
                {
                    Debug.Log("Enemy Hit Enemy" + GameManager.Instance.playerBehaviour.CalculateArrowDamage(projectileData.damage, _enemyPierced, _duration));
                    if (_pierceCount > 0)
                    {
                        _pierceCount--;
                        _enemyPierced++;
                    }
                    else
                    {
                        Destroy(gameObject);
                    }
                }
                break;
            case "Wall":
                if (projectileData.isBouncy)
                {
                    if (_bounceCount > 0)
                    {
                        _bounceCount--;
                        _direction = Vector3.Reflect(_direction, col.transform.up);
                        gameObject.transform.up = _direction;
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

    public void SetDirection(Vector3 direction)
    {
        _direction = direction;
    }
    
    public float Duration
    {
        get => _duration;
        set => _duration = value;
    }
}
