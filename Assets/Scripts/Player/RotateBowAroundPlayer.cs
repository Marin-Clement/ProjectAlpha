using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateBowAroundPlayer : MonoBehaviour
{
    [Header("Bow Rotation")]

    [SerializeField] private float rotationSpeed = 5f;
    [SerializeField] private float distanceToPlayer = 1.5f;
    private SpriteRenderer _spriteRenderer;

    private Transform playerTransform;

    void Start()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        playerTransform = transform.parent;
    }

    void Update()
    {
        Vector3 mousePosScreen = Input.mousePosition;
        Vector3 mousePosWorld = Camera.main.ScreenToWorldPoint(new Vector3(mousePosScreen.x, mousePosScreen.y, Camera.main.transform.position.z));
        Vector3 direction = mousePosWorld - playerTransform.position;
        direction.z = 0f;
        float angle = Mathf.LerpAngle(transform.eulerAngles.z, Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg, rotationSpeed * Time.deltaTime);
        transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));

        if (angle > 90 && angle < 270)
        {
            _spriteRenderer.flipY = true;
        }
        else
        {
            _spriteRenderer.flipY = false;
        }

        Vector3 bowPosition = playerTransform.position + direction.normalized * distanceToPlayer;
        transform.position = bowPosition;
    }
}
