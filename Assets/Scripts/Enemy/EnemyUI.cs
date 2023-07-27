using UnityEngine;
using UnityEngine.UI;

public class EnemyUI : MonoBehaviour
{
    [Header("Health UI")]
    [SerializeField] private Slider healthBar;
    [SerializeField] private Slider healthTempBar;

    // Enemy

    private EnemyBehaviour _enemyBehaviour;

    
    void Start()
    {
        _enemyBehaviour = GetComponent<EnemyBehaviour>();
    }

    void Update()
    {
        UpdateHealthUI();
    }

    public void UpdateHealthUI()
    {
        healthBar.maxValue = _enemyBehaviour.MaxHealth;
        healthTempBar.maxValue = _enemyBehaviour.MaxHealth;
        healthBar.value = Mathf.Lerp(healthBar.value, _enemyBehaviour.Health, 0.01f);
        healthTempBar.value = Mathf.Lerp(healthTempBar.value, healthBar.value, 0.005f);
    }
}
