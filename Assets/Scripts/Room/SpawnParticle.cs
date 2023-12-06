using UnityEngine;
using UnityEngine.Rendering.Universal;

public class SpawnParticle : MonoBehaviour
{
    private Light2D _light;
    private float _timer = 1f;

    private void Start()
    {
        _light = GetComponentInChildren<Light2D>();
    }

    void Update()
    {
        _light.intensity -= Time.deltaTime * 2;
        _light.pointLightOuterRadius -= Time.deltaTime * 2;
        _timer -= Time.deltaTime;
        if (_timer <= 0)
        {
            Destroy(gameObject);
        }
    }
}