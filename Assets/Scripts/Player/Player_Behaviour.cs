using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Behaviour : MonoBehaviour
{
    [Header("Player Stats")] 

    [SerializeField] private float maxHealth = 100;
    private float health;

    [SerializeField] private int lvl = 1;

    private bool _vulnerable = true;

    private Player_Movement _playerMovement;

    [SerializeField] private Player_Camera playerCamera;

    // Attack stats
    [Header("Attack Stats")] 
    
    [SerializeField] private int damage = 10;
    [SerializeField] private int attackRange = 1; // Default attack range is 1
    [SerializeField] private int criticalChance = 0; // In percentage
    [SerializeField] private int criticalDamage = 10; // In percentage
    [SerializeField] private int armorPenetration = 0; // In percentage
    
    // Defence stats
    [Header("Defence Stats")] [SerializeField]
    private int armor = 10; // Default armor is 10

    [SerializeField] private int magicResistance = 10; // Default magic resistance is 10
    [SerializeField] private int dodge = 0;

    private void Start()
    {
        _playerMovement = GetComponent<Player_Movement>();
        GameManager.Instance.playerBehaviour = this;
        health = maxHealth;
    }

    // Stats Functions
    private void CalculateStatsPerLevels()
    {
        return;
    }

    private void LevelUp()
    {
        lvl++;
        CalculateStatsPerLevels();
    }

    private void TakeDamage(int damage)
    {
        health -= damage;
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.CompareTag("Enemy") || col.gameObject.CompareTag("EnemyProjectile"))
        {
            playerCamera.ShakeCamera(0.2f, 0.1f);
            _playerMovement.Knockback(col.transform.position);
            if (!_vulnerable) return;
            if (col.gameObject.CompareTag("Enemy"))
            {
                TakeDamage(10);
            }

            if (col.gameObject.CompareTag("EnemyProjectile"))
            {
                TakeDamage(10);
                Destroy(col.gameObject);
            }
        }
        StartCoroutine(InvincibilityTimer());
    }

    private IEnumerator InvincibilityTimer()
    {
        _vulnerable = false;
        yield return new WaitForSeconds(0.5f);
        _vulnerable = true;
    }
    
    public List<object> CalculateArrowDamage(float arrowDamage, int enemyPierce, float holdTime)
    {
        List<object> damageInfo = new List<object>();

        float calculatedDamage = (((arrowDamage * (1 + holdTime)) * damage * 0.2f) / (1 + (enemyPierce * 0.4f)));
        bool isCriticalHit = criticalChance > Random.Range(0, 100);

        if (isCriticalHit)
        {
            calculatedDamage *= (1 + (criticalDamage * 0.01f));
        }

        damageInfo.Add(calculatedDamage);
        damageInfo.Add(isCriticalHit);
        
        return damageInfo;
    }

    public float Health
    {
        get => health;
        set => health = value;
    }

    public float MaxHealth
    {
        get => maxHealth;
        set => maxHealth = value;
    }
}