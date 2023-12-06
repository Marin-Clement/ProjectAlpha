using UnityEngine;
using UnityEngine.UI;

public class EnemyUI : MonoBehaviour
{
    [Header("Health UI")] [SerializeField] private Slider healthBar;
    [SerializeField] private Slider healthTempBar;

    // Enemy

    private Health _enemyHealth;


    void Start()
    {
        _enemyHealth = GetComponent<Health>();
    }

    void Update()
    {
        UpdateHealthUI();
    }

    public void UpdateHealthUI()
    {
        healthBar.maxValue = _enemyHealth.MaxHealth;
        healthTempBar.maxValue = _enemyHealth.MaxHealth;
        healthBar.value = Mathf.Lerp(healthBar.value, _enemyHealth.HealthValue, 0.01f);
        healthTempBar.value = Mathf.Lerp(healthTempBar.value, healthBar.value, 0.005f);
    }
}