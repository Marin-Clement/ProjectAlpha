using UnityEngine;

public class Projectile_Behaviour : MonoBehaviour
{
    // Projectile Variables
    private Rigidbody _rigidbody;
    private Vector3 _direction;

    public Projectile_Data projectileData;
    

    // Homing Variables
    private GameObject _target;
    
    // Explosive Variables
    private float currentExplosionTime;
    private float spawnTime = 0.3f;
    

    private void Start()
    {
        _target = GameObject.FindGameObjectWithTag("Enemy");
        currentExplosionTime = projectileData.timeToExplode;
        _rigidbody = GetComponent<Rigidbody>();
        if (projectileData.lifeTime > 0)
        {
            Destroy(gameObject, projectileData.lifeTime);
        }
    }

    private void Update()
    {
        if (projectileData.isHoming)
        {
            if (_target)
            {
                spawnTime -= Time.deltaTime;
                if (spawnTime <= 0)
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
        currentExplosionTime -= Time.deltaTime;
        if (currentExplosionTime <= 0)
        {
            Explode();
            currentExplosionTime = projectileData.timeToExplode;
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
    
    public void SetDirection(Vector3 direction)
    {
        _direction = direction;
    }
    
    public void SetTarget(GameObject target)
    {
        _target = target;
    }
}
