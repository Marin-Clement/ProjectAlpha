using UnityEngine;
using System.Collections;

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
    
    
    public IEnumerator Shake(float duration, float magnitude)
    {
        transform.position = Vector3.Lerp(transform.position , player.transform.position + _offset, cameraSpeed * Time.deltaTime);
        float elapsed = 0f;
        while (elapsed < duration)
        {
            float x = Random.Range(-1f, 1f) * magnitude;
            float y = Random.Range(-1f, 1f) * magnitude;

            transform.position = new Vector3(transform.position.x + x, transform.position.y + y, transform.position.z);
            elapsed += Time.deltaTime;
            yield return 0;
        }
        transform.position = Vector3.Lerp(transform.position , player.transform.position + _offset, cameraSpeed * Time.deltaTime);
        _isShaking = false;
    }
}
