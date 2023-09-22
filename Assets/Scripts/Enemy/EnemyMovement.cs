using UnityEngine;
using Random = UnityEngine.Random;

public class EnemyMovement : MonoBehaviour
{
    private EnemyBehaviour _enemyBehaviour;
    public float raycastDistance = 10.0f;
    public LayerMask obstacleLayer;
    public float minDistanceToObstacle = 10f;

    private Vector2[] directionVectors;
    private float[] directionWeights;
    int highestWeightIndex = 0;

    private float _rangePersonalSpace;

    //temp
    private SpriteRenderer _spriteRenderer;

    private void Start()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _enemyBehaviour = GetComponent<EnemyBehaviour>();
        _rangePersonalSpace = _enemyBehaviour.AttackRange * 0.8f;
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

    public void Routine()
    {
        ResetWeights();
        CheckForObstacles();
        FindBestDirection();
        MoveIntoBestDirection();
        if (transform.position.x > GameManager.Instance.player.transform.position.x)
        {
            _spriteRenderer.flipX = true;
        }
        else
        {
            _spriteRenderer.flipX = false;
        }
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
            if (Physics2D.Raycast(transform.position, directionVectors[i], raycastDistance, obstacleLayer))
            {
                directionWeights[i] = 0.0f;
            }
            else
            {
                // No obstacle found in this direction, so add weight based on distance to the player
                var distanceToPlayer = Vector2.Distance((transform.position + (Vector3)directionVectors[i] * raycastDistance), GameManager.Instance.player.transform.position);
                // If the distance is smaller than the minimum distance to an obstacle, set the weight to 0
                if (distanceToPlayer < minDistanceToObstacle)
                {
                    directionWeights[i] = 0.0f;
                }
                else
                {
                    directionWeights[i] = 1.0f / distanceToPlayer;
                }
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
        if (Vector2.Distance(transform.position, GameManager.Instance.player.transform.position) < _enemyBehaviour.AttackRange)
        {
            if (_enemyBehaviour.IsRanged && !Physics2D.Raycast(transform.position, GameManager.Instance.player.transform.position - transform.position, _enemyBehaviour.AttackRange, obstacleLayer))
            {
                // if player in range of personal space move away
                if (Vector2.Distance(transform.position, GameManager.Instance.player.transform.position) < _rangePersonalSpace)
                {
                    // Debug distance to player
                    Debug.DrawLine(transform.position, GameManager.Instance.player.transform.position, Color.red);
                    Vector2 direction = directionVectors[highestWeightIndex];
                    direction = new Vector2(direction.x + Random.Range(-0.1f, 0.1f), direction.y + Random.Range(-0.1f, 0.1f));
                    transform.position -= (Vector3)direction * (_enemyBehaviour.MovementSpeed * Time.deltaTime);
                }
                else
                {
                    _enemyBehaviour.enemyState = EnemyBehaviour.EnemyState.Attack;
                }
            }
            else
            {
                Vector2 direction = directionVectors[highestWeightIndex];
                direction = new Vector2(direction.x + Random.Range(-0.1f, 0.1f), direction.y + Random.Range(-0.1f, 0.1f));
                transform.position += (Vector3)direction * (_enemyBehaviour.MovementSpeed * Time.deltaTime);
            }
            if (_enemyBehaviour.IsMelee)
            {
            }
        }
        else
        {
            Vector2 direction = directionVectors[highestWeightIndex];
            Vector2 newDirection = direction + new Vector2(Random.Range(-0.1f, 0.1f), Random.Range(-0.1f, 0.1f));
            direction = Vector2.Lerp(direction, newDirection, 0.2f); // 0.2f is the blending factor
            transform.position += (Vector3)direction * (_enemyBehaviour.MovementSpeed * Time.deltaTime);
        }
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