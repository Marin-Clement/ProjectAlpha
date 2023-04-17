using System;
using System.Collections;
using UnityEditor.Build;
using UnityEngine;

public class Player_Behaviour : MonoBehaviour
{
    [Header("Player Stats")]
    [SerializeField]
    private int healh = 50; // Default lives is 50
    [SerializeField]
    private int lvl = 1; // Default level is 1

    private bool _vulnerable = true;

    private Player_Movement _playerMovement;
    [SerializeField]
    private Player_Camera playerCamera;
    // Attack stats
    [Header("Attack Stats")]
    [SerializeField]
    private int damage = 10; // Default damage is 10
    [SerializeField]
    private int attackSpeed = 1; // Default attack speed is 1
    [SerializeField]
    private int attackRange = 1; // Default attack range is 1
    [SerializeField]
    private int criticalChance = 0;  // Default critical chance is 0
    [SerializeField]
    private int criticalDamage = 10; // Default critical damage is 10
    [SerializeField]
    private int armorPenetration = 0; // Default armor penetration is 0

    [Header("On Hit Effects")]
    [SerializeField]
    private bool bleed = false;
    [SerializeField]
    private bool poison = false;
    



    // Defence stats
    [Header("Defence Stats")]
    [SerializeField]
    private int armor = 10; // Default armor is 10
    [SerializeField]
    private int magicResistance = 10; // Default magic resistance is 10
    [SerializeField]
    private int dodge = 0;

    private void Start()
    {
        _playerMovement = GetComponent<Player_Movement>();
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
        healh -= damage;
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.CompareTag("Enemy") || col.gameObject.CompareTag("EnemyProjectile") && _vulnerable)
        {
            TakeDamage(10);
            if (col.gameObject.CompareTag("Enemy"))
            {
                playerCamera.Shake(0.5f, 1f);
                _playerMovement.Knockback(col.transform.position);
            }
            if (col.gameObject.CompareTag("EnemyProjectile"))
            {
                Destroy(col.gameObject);
            }
        }
        StartCoroutine(InvencibilityTimer());
    }

    private IEnumerator InvencibilityTimer()
    {
        _vulnerable = false;
        yield return new WaitForSeconds(0.5f);
        _vulnerable = true;
    }
}
