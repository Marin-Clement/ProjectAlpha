using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    private EnemyBehaviour _enemyBehaviour;
    public float raycastDistance = 10f;
    public LayerMask obstacleLayer;
    public float minDistanceToObstacle = 1f;

    private Vector2[] _directionVectors;
    private float[] _directionWeights;
    private int _highestWeightIndex = 0;

    private float _rangePersonalSpace = 7f;

    private GameObject[] _enemies;
    private GameObject _player;

    private void Start()
    {
        _enemyBehaviour = GetComponent<EnemyBehaviour>();

        // Define the direction vectors for the enemy
        CreateDirectionVectors(8);
    }

    // Create the direction vectors for the enemy
    private void CreateDirectionVectors(int numberOfDirections)
    {
        _directionVectors = new Vector2[numberOfDirections];
        _directionWeights = new float[numberOfDirections];
        float angle = 2 * Mathf.PI / numberOfDirections;
        for (int i = 0; i < numberOfDirections; i++)
        {
            _directionVectors[i] = new Vector2(Mathf.Cos(angle * i), Mathf.Sin(angle * i));
        }
    }

    private readonly float _maxTime = 0.4f;
    private float _timer;

    // Update is called once per frame
    public void Routine()
    {
        _timer -= Time.deltaTime;
        if (_timer <= 0)
        {
            _timer = _maxTime;
            ResetWeights();
            FindBestDirection();
        }

        CheckForObstacle();
        MoveIntoBestDirection();
        if (InRange())
        {
            _enemyBehaviour.enemyState = EnemyBehaviour.EnemyState.Attack;
            Debug.DrawLine(transform.position, GameManager.Instance.player.transform.position, Color.blue);
        }
    }

    private void ResetWeights()
    {
        for (int i = 0; i < _directionWeights.Length; i++)
        {
            _directionWeights[i] = 1;
        }
    }

    private void FindBestDirection()
    {
        CalculateWeights();

        // Find the highest weight
        _highestWeightIndex = 0;
        for (int i = 0; i < _directionWeights.Length; i++)
        {
            if (_directionWeights[i] > _directionWeights[_highestWeightIndex])
            {
                _highestWeightIndex = i;
            }
        }
    }

    private bool InRange()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        Vector2 currentPosition = transform.position;
        Vector2 playerPosition = player.transform.position;
        float distance = CalculateDistance(currentPosition, playerPosition);
        return distance < _enemyBehaviour.AttackRange;
    }

    private float CalculateDistance(Vector2 positionA, Vector2 positionB)
    {
        return Vector2.Distance(positionA, positionB);
    }

    private void CalculateWeights()
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        GameObject player = GameObject.FindGameObjectWithTag("Player");

        Vector2 currentPosition = transform.position;

        // Check for other enemies (avoid self)
        foreach (var enemy in enemies)
        {
            if (enemy == gameObject) continue;

            Vector2 enemyPosition = enemy.transform.position;
            Vector2 direction = enemyPosition - currentPosition;
            float distance = CalculateDistance(currentPosition, enemyPosition);
            Debug.DrawLine(currentPosition, enemyPosition, Color.red);

            if (distance < _rangePersonalSpace)
            {
                direction.Normalize();
                float weight = 1 - (distance / _rangePersonalSpace);
                AddWeight(direction, -weight);
            }
        }

        // Check for player
        Vector2 playerPosition = player.transform.position;
        Vector2 playerDirection = playerPosition - currentPosition;
        float playerDistance = CalculateDistance(currentPosition, playerPosition);

        if (playerDistance < _rangePersonalSpace)
        {
            playerDirection.Normalize();
            float weight = 1 - (playerDistance / _rangePersonalSpace);
            AddWeight(playerDirection, weight);

            // add weight to the opposite direction of the player if no wall is in the way
            RaycastHit2D hit = Physics2D.Raycast(transform.position, playerDirection, raycastDistance, obstacleLayer);
            if (!hit.collider)
            {
                AddWeight(-playerDirection, 1);
            }
        }

        if (_enemyBehaviour.IsRanged)
        {
            // Go in max range
            if (playerDistance > _enemyBehaviour.AttackRange)
            {
                float weight = 1 - (_enemyBehaviour.AttackRange / playerDistance);
                AddWeight(playerDirection, weight);
            }
            else
            {
                // Randomly move
                if (Random.Range(0, 4) == 0)
                {
                    Vector2 randomDirection = new Vector2(Random.Range(-1f, 1f), Random.Range(-1f, 1f));
                    AddWeight(randomDirection, 1);
                }
            }
        }
    }

    private void CheckForObstacle()
    {
        for (int i = 0; i < _directionVectors.Length; i++)
        {
            Vector2 direction = _directionVectors[i];
            RaycastHit2D hit = Physics2D.Raycast(transform.position, direction, raycastDistance, obstacleLayer);
            if (hit.collider != null)
            {
                // Remove weight if obstacle is closer
                float weight = 1 - (hit.distance / raycastDistance);
                AddWeight(direction, -weight);
            }
        }
    }

    private void AddWeight(Vector2 direction, float weight)
    {
        for (int i = 0; i < _directionVectors.Length; i++)
        {
            // Take the closest direction
            if (Vector2.Dot(direction, _directionVectors[i]) > 0.5f)
            {
                _directionWeights[i] += weight;
            }
        }
    }

    private void MoveIntoBestDirection()
    {
        _highestWeightIndex = 0;
        for (int i = 0; i < _directionWeights.Length; i++)
        {
            if (_directionWeights[i] > _directionWeights[_highestWeightIndex])
            {
                _highestWeightIndex = i;
            }
        }

        Vector2 direction = _directionVectors[_highestWeightIndex];
        Move(direction);
    }

    private void Move(Vector2 direction)
    {
        var position = transform.position;
        Vector2 targetPosition =
            (Vector2)position + direction * (_enemyBehaviour.MovementSpeed * Time.deltaTime);
        position = Vector2.Lerp(position, targetPosition, 0.4f);
        transform.position = position;
    }
}