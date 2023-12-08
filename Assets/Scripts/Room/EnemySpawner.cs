using UnityEngine;
using UnityEngine.Rendering.Universal;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] private GameObject spawnParticle;
    private Light2D spawnLight;
    private float _spawnTimer = 1f;
    private bool _canSpawn;

    public void SpawnEnemy(GameObject enemy)
    {
        Instantiate(spawnParticle, transform.position, Quaternion.identity);
        Instantiate(enemy, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }

    private void Start()
    {
        spawnLight = GetComponent<Light2D>();
        if (!_canSpawn)
        {
            spawnLight.intensity = 0;
            spawnLight.pointLightOuterRadius = 0;
        }
    }

    private void Update()
    {
        if (_canSpawn)
        {
            _spawnTimer -= Time.deltaTime;
            spawnLight.intensity = _spawnTimer * 2;
            spawnLight.pointLightInnerRadius = _spawnTimer * 2;
            spawnLight.pointLightOuterRadius = _spawnTimer * 10;
        }
    }

    public void SetCanSpawn(bool canSpawn)
    {
        _canSpawn = canSpawn;
    }
}