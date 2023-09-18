using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public void SpawnEnemy(GameObject enemy)
    {
        Instantiate(enemy , transform.position, Quaternion.identity);
        Destroy(gameObject);
    }
}