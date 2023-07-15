using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Behaviour : MonoBehaviour
{
    [Header("Player Stats")] [SerializeField]
    private int health = 50; // Default lives is 50

    [SerializeField] private int lvl = 1; // Default level is 1

    private bool _vulnerable = true;

    private Player_Movement _playerMovement;

    [SerializeField] private Player_Camera playerCamera;

    // Attack stats
    [Header("Attack Stats")] 
    
    [SerializeField] private int damage = 10; // Default damage is 10
    [SerializeField] private int attackRange = 1; // Default attack range is 1
    [SerializeField] private int criticalChance = 0; // Default critical chance is 0
    [SerializeField] private int criticalDamage = 10; // Default critical damage is 10
    [SerializeField] private int armorPenetration = 0; // Default armor penetration is 0
    
    // Defence stats
    [Header("Defence Stats")] [SerializeField]
    private int armor = 10; // Default armor is 10

    [SerializeField] private int magicResistance = 10; // Default magic resistance is 10
    [SerializeField] private int dodge = 0;

    private void Start()
    {
        _playerMovement = GetComponent<Player_Movement>();
        GameManager.Instance.playerBehaviour = this;
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
        if (col.gameObject.CompareTag("Enemy") || col.gameObject.CompareTag("EnemyProjectile") && _vulnerable)
        {
            TakeDamage(10);
            if (col.gameObject.CompareTag("Enemy"))
            {
                playerCamera.Shake(0.5f, 10f);
                _playerMovement.Knockback(col.transform.position);
            }

            if (col.gameObject.CompareTag("EnemyProjectile"))
            {
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
        List<object> result = new List<object>();

        float calculatedDamage = (((arrowDamage * (1 + (holdTime / 10))) * damage * 0.2f) / (1 + (enemyPierce * 0.4f)));
        bool isCriticalHit = criticalChance > Random.Range(0, 100);

        if (isCriticalHit)
        {
            calculatedDamage *= (1 + (criticalDamage * 0.1f));
        }

        result.Add(calculatedDamage);
        result.Add(isCriticalHit);
        
        return result;
    }
}