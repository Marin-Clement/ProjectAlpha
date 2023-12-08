using UnityEngine;
using UnityEngine.Rendering.Universal;

public class SpawnParticle : MonoBehaviour
{
    private Light2D _light;
    private float _timer = 1f;

    private void Start()
    {
        _light = GetComponent<Light2D>();
        _light.intensity = 1f;
        _light.pointLightOuterRadius = 0f;
    }

    void Update()
    {
        _light.intensity -= Time.deltaTime;
        _light.pointLightOuterRadius += Time.deltaTime * 10f;
        _light.pointLightInnerRadius += Time.deltaTime * 5f;
        _timer -= Time.deltaTime;
        if (_timer <= 0)
        {
            Destroy(gameObject);
        }
    }
}