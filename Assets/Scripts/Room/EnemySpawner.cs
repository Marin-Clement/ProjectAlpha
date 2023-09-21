using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    private SpriteRenderer _spawnPreview;
    private Transform _spawnPreviewTransform;
    private float _spawnTimer = 1f;
    private bool _canSpawn = false;

    public void SpawnEnemy(GameObject enemy)
    {
        Instantiate(enemy , transform.position, Quaternion.identity);
        Destroy(gameObject);
    }

    private void Start()
    {
        _spawnPreview = GetComponentInChildren<SpriteRenderer>();
        _spawnPreviewTransform = _spawnPreview.transform;
        if (!_canSpawn)
        {
            _spawnPreview.color = new Color(1, 0, 0, 0);
        }
    }

    private void Update()
    {
        if (_canSpawn)
        {
            _spawnTimer -= Time.deltaTime;
            _spawnPreview.color = new Color(1, 0, 0, _spawnTimer);
            _spawnPreviewTransform.localScale = new Vector3(_spawnTimer * 3 ,_spawnTimer * 3, 1);
        }
    }

    public void SetCanSpawn(bool canSpawn)
    {
        _canSpawn = canSpawn;
    }
}