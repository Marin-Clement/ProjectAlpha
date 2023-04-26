using System;
using System.Collections;
using System.Collections.Generic;
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

    private void Start()
    {
        _enemyBehaviour = GetComponent<EnemyBehaviour>();
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
        // Reset the direction weights
        for (int i = 0; i < directionWeights.Length; i++)
        {
            directionWeights[i] = 0.0f;
        }

        // Check each direction for obstacles
        for (int i = 0; i < directionVectors.Length; i++)
        {
            Vector3 targetPos = transform.position + (Vector3)directionVectors[i] * raycastDistance;
            Collider2D obstacle = Physics2D.OverlapCircle(targetPos, minDistanceToObstacle, obstacleLayer);
            if (obstacle != null)
            {
                // Enemy is too close to an obstacle in this direction, so skip it
                continue;
            }
            RaycastHit2D hit = Physics2D.Raycast(transform.position, directionVectors[i], raycastDistance, obstacleLayer);
            if (hit.collider != null)
            {
                directionWeights[i] = 0.0f;
            }
            else
            {
                // No obstacle found in this direction, so add weight based on distance to the player
                float distanceToPlayer = Vector2.Distance((transform.position + (Vector3)directionVectors[i] * raycastDistance), GameManager.Instance.playerBehaviour.transform.position);
                directionWeights[i] = 1.0f / distanceToPlayer;
            }
        }
        
        float highestWeight = 0.0f;
        for (int i = 0; i < directionWeights.Length; i++)
        {
            if (directionWeights[i] > highestWeight)
            {
                highestWeight = directionWeights[i];
                highestWeightIndex = i;
            }
        }
        
        // Move in the direction with the highest weight
        transform.position += (Vector3)directionVectors[highestWeightIndex] * (_enemyBehaviour.MovementSpeed * Time.deltaTime);
    }

    private void OnDrawGizmosSelected()
    {
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
