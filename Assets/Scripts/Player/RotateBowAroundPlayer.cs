using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateBowAroundPlayer : MonoBehaviour
{
    [Header("Bow Rotation")] [SerializeField]
    private float rotationSpeed = 5f;

    [SerializeField] private float distanceToPlayer = 1.5f;
    private SpriteRenderer _spriteRenderer;

    private Transform playerTransform;
    private Camera playerCamera;

    void Start()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        playerCamera = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
        playerTransform = transform.parent;
    }

    void Update()
    {
        Vector3 mousePosScreen = Input.mousePosition;
        Vector3 mousePosWorld = playerCamera.GetComponent<Camera>().ScreenToWorldPoint(mousePosScreen);
        Vector3 direction = mousePosWorld - playerTransform.position;
        direction.z = 0f;
        float angle = Mathf.LerpAngle(transform.eulerAngles.z, Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg,
            rotationSpeed * Time.deltaTime);
        transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));

        _spriteRenderer.flipY = angle is > 90 and < 270;

        Vector3 bowPosition = playerTransform.position + direction.normalized * distanceToPlayer;
        transform.position = bowPosition;
    }
}