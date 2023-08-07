using UnityEngine;
using Random = UnityEngine.Random;

public class EnemyMovement : MonoBehaviour
{
    private EnemyBehaviour _enemyBehaviour;
    public float raycastDistance = 2.0f;
    public LayerMask obstacleLayer;
    public float minDistanceToObstacle = 0.5f;

    private Vector2[] directionVectors;
    private float[] directionWeights;
    int highestWeightIndex = 0;

    private float _rangePersonalSpace;

    private void Start()
    {
        _enemyBehaviour = GetComponent<EnemyBehaviour>();
        _rangePersonalSpace = _enemyBehaviour.AttackRange * 0.5f;
        // Define the direction vectors
        directionVectors = new Vector2[] {
            new Vector2(0.0f, 1.0f),
            new Vector2(0.5f, 0.866f),
            new Vector2(0.866f, 0.5f),
            new Vector2(1.0f, 0.0f),
            new Vector2(0.866f, -0.5f),
            new Vector2(0.5f, -0.866f),
            new Vector2(0.0f, -1.0f),
            new Vector2(-0.5f, -0.866f),
            new Vector2(-0.866f, -0.5f),
            new Vector2(-1.0f, 0.0f),
            new Vector2(-0.866f, 0.5f),
            new Vector2(-0.5f, 0.866f)
        };

        // Initialize the direction weights
        directionWeights = new float[directionVectors.Length];
    }

    private void Update()
    {
        ResetWeights();
        CheckForObstacles();
        FindBestDirection();
        MoveIntoBestDirection();
    }
    
    private void ResetWeights()
    {
        // Reset the direction weights
        for (int i = 0; i < directionWeights.Length; i++)
        {
            directionWeights[i] = 0.0f;
        }
    }
    
    private void CheckForObstacles()
    {
        // Check each direction for obstacles
        for (int i = 0; i < directionVectors.Length; i++)
        {
            Vector3 targetPos = transform.position + (Vector3)directionVectors[i] * raycastDistance;
            if (Physics2D.OverlapCircle(targetPos, minDistanceToObstacle, obstacleLayer))
            {
                _enemyBehaviour.enemyStatus = "Stop walls!";
                continue;
            }
            if (Physics2D.Raycast(transform.position, directionVectors[i], raycastDistance, obstacleLayer))
            {
                directionWeights[i] = 0.0f;
            }
            else
            {
                _enemyBehaviour.enemyStatus = "Chasing player!";
                // No obstacle found in this direction, so add weight based on distance to the player
                // var distanceToPlayer = Vector2.Distance((transform.position + (Vector3)directionVectors[i] * raycastDistance), GameManager.Instance.playerBehaviour.transform.position);
                // directionWeights[i] = 1.0f / distanceToPlayer;
            }
        }
    }
    
    private void FindBestDirection()
    {
        var highestWeight = 0.0f;
        // Find the direction with the highest weight
        for (var i = 0; i < directionWeights.Length; i++)
        {
            if (!(directionWeights[i] > highestWeight)) continue;
            highestWeight = directionWeights[i];
            highestWeightIndex = i;
        }
    }

    private void MoveIntoBestDirection()
    {
        // Move into the direction with the highest weight but if in range of firing
        // if (Vector2.Distance(transform.position, GameManager.Instance.playerBehaviour.transform.position) < _enemyBehaviour.AttackRange)
        // {
        //     if (_enemyBehaviour.IsRanged && !Physics2D.Raycast(transform.position, GameManager.Instance.playerBehaviour.transform.position - transform.position, _enemyBehaviour.AttackRange, obstacleLayer))
        //     {
        //         // if player in range of personal space move away
        //         if (Vector2.Distance(transform.position, GameManager.Instance.playerBehaviour.transform.position) < _rangePersonalSpace)
        //         {
        //             _enemyBehaviour.enemyStatus = "Personal space!";
        //             Vector2 direction = directionVectors[highestWeightIndex];
        //             direction = new Vector2(direction.x + Random.Range(-0.1f, 0.1f), direction.y + Random.Range(-0.1f, 0.1f));
        //             transform.position -= (Vector3)direction * (_enemyBehaviour.MovementSpeed * Time.deltaTime);
        //         }
        //         else
        //         {
        //             _enemyBehaviour.enemyStatus = "Firing!";
        //         }
        //     }
        //     else
        //     {
        //         _enemyBehaviour.enemyStatus = "Moving!";
        //         Vector2 direction = directionVectors[highestWeightIndex];
        //         direction = new Vector2(direction.x + Random.Range(-0.1f, 0.1f), direction.y + Random.Range(-0.1f, 0.1f));
        //         transform.position += (Vector3)direction * (_enemyBehaviour.MovementSpeed * Time.deltaTime);
        //     }
        //     if (_enemyBehaviour.IsMelee)
        //     {
        //         _enemyBehaviour.enemyStatus = "Melee!";
        //     }
        // }
        // else
        // {
        //     _enemyBehaviour.enemyStatus = "Moving!";
        //     Vector2 direction = directionVectors[highestWeightIndex];
        //     Vector2 newDirection = direction + new Vector2(Random.Range(-0.1f, 0.1f), Random.Range(-0.1f, 0.1f));
        //     direction = Vector2.Lerp(direction, newDirection, 0.2f); // 0.2f is the blending factor
        //     transform.position += (Vector3)direction * (_enemyBehaviour.MovementSpeed * Time.deltaTime);
        // }
    }   
    
    private void OnDrawGizmos()
    {
        if (!Application.isPlaying) return;
        // Draw the direction vectors with their weights as colors (green best, red worst)
        for (int i = 0; i < directionVectors.Length; i++)
        {
            if (directionWeights[i] > 0.0f)
            {
                Gizmos.color = Color.green;
                Gizmos.DrawLine(transform.position, transform.position + ((Vector3)directionVectors[i] * raycastDistance ) * (directionWeights[i] * 5.0f));
            }
            else
            {
                Gizmos.color = Color.red;
                Gizmos.DrawLine(transform.position, transform.position + (Vector3)directionVectors[i] * raycastDistance);
            }
        }
    }
}