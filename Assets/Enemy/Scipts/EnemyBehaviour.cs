using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBehaviour : MonoBehaviour
{
    [SerializeField] private EnemyData enemyData;

    [Header("Enemy Data")]
    private Sprite enemySprite;
    private string enemyName;
    private GameObject enemyProjectile;
   
    // Enemy stats
    [Header("Enemy Base Stats")]
    private int health;
    private int lvl;

    // Attack stats
    [Header("Attack Stats")]
    private int damage;
    private int attackRange;
    private int criticalChance;
    private int criticalDamage;
    private int armorPenetration;
    private float attackSpeed;
   
    // Defence stats
    [Header("Defence Stats")]
    private int armor;
    private int magicResistance;
   
    // Movement stats
    [Header("Movement Stats")]
    private float movementSpeed;
   
    // Enemy type
    [Header("Enemy Type")]
    private bool _isRanged;
    private bool _isMelee;
    private bool _isMagic;
    private bool _isPhysical;

    private void Start()
    {
        SetEnemyVariables();
    }

    private void SetEnemyVariables()
    {
        enemySprite = enemyData.enemySprite;
        enemyName = enemyData.enemyName;
        enemyProjectile = enemyData.enemyProjectile;
        health = enemyData.health;
        lvl = enemyData.lvl;
        damage = enemyData.damage;
        attackRange = enemyData.attackRange;
        criticalChance = enemyData.criticalChance;
        criticalDamage = enemyData.criticalDamage;
        armorPenetration = enemyData.armorPenetration;
        attackSpeed = enemyData.attackSpeed;
        armor = enemyData.armor;
        magicResistance = enemyData.magicResistance;
        movementSpeed = enemyData.movementSpeed;
        _isRanged = enemyData.isRanged;
        _isMelee = enemyData.isMelee;
        _isMagic = enemyData.isMagic;
        _isPhysical = enemyData.isPhysical;
    }
}
