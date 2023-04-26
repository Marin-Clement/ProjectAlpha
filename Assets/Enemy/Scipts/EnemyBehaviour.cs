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
    
    // Setters Getters Functions
    public Sprite Sprite
    {
        get => enemySprite;
        set => enemySprite = value;
    }
    
    public string EnemyName
    {
        get => enemyName;
        set => enemyName = value;
    }
    
    public GameObject EnemyProjectile
    {
        get => enemyProjectile;
        set => enemyProjectile = value;
    }
    
    // Enemy stats
    
    public int Health
    {
        get => health;
        set => health = value;
    }
    
    public int Lvl
    {
        get => lvl;
        set => lvl = value;
    }
    
    // Attack stats
    
    public int Damage
    {
        get => damage;
        set => damage = value;
    }
    
    public int AttackRange
    {
        get => attackRange;
        set => attackRange = value;
    }
    
    public int CriticalChance
    {
        get => criticalChance;
        set => criticalChance = value;
    }
    
    public int CriticalDamage
    {
        get => criticalDamage;
        set => criticalDamage = value;
    }
    
    public int ArmorPenetration
    {
        get => armorPenetration;
        set => armorPenetration = value;
    }
    
    public float AttackSpeed
    {
        get => attackSpeed;
        set => attackSpeed = value;
    }
    
    // Defence stats
    
    public int Armor
    {
        get => armor;
        set => armor = value;
    }
    
    public int MagicResistance
    {
        get => magicResistance;
        set => magicResistance = value;
    }
    
    // Movement stats
    
    public float MovementSpeed
    {
        get => movementSpeed;
        set => movementSpeed = value;
    }
    
    // Enemy type
    
    public bool IsRanged
    {
        get => _isRanged;
        set => _isRanged = value;
    }
    
    public bool IsMelee
    {
        get => _isMelee;
        set => _isMelee = value;
    }
    
    public bool IsMagic
    {
        get => _isMagic;
        set => _isMagic = value;
    }
    
    public bool IsPhysical
    {
        get => _isPhysical;
        set => _isPhysical = value;
    }
}
