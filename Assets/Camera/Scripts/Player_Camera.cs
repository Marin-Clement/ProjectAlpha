using System;
using UnityEngine;
using Random = UnityEngine.Random;

public class Player_Camera : MonoBehaviour
{
    public Player_Movement player;
    
    private Vector3 _offset;
    
    [SerializeField]
    private int cameraSpeed = 5;
    
    private bool _isShaking;
    

    void Start()
    {
        
        _offset = new Vector3(0, 0, -10);
    }

    private void FixedUpdate()
    {
        if (!_isShaking)
        {
            FollowPlayer();
        }
    }


    private void FollowPlayer()
    {
        transform.position = Vector3.Lerp(transform.position , player.transform.position + _offset, cameraSpeed * Time.deltaTime);
    }
    
    
    
    public void Shake(float duration, float magnitude)
    {
        _isShaking = true;
        Vector3 originalPos = transform.localPosition;
        float elapsed = 0.0f;
        while (elapsed < duration)
        {
            float x = Random.Range(-1f, 1f) * magnitude;
            float y = Random.Range(-1f, 1f) * magnitude;
            transform.localPosition = new Vector3(x, y, originalPos.z);
            elapsed += Time.deltaTime;
        }
        transform.localPosition = originalPos;
        _isShaking = false;
    }
}
