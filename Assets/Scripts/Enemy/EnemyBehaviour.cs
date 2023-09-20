using UnityEngine;

public class EnemyBehaviour : MonoBehaviour
{
    public enum EnemyState
    {
        Idle,
        Chase,
        Attack,
        Dead
    }

    [Header("Enemy Data")]
    private Sprite _enemySprite;
    private string _enemyName;
    private GameObject _enemyProjectile;
    public EnemyState enemyState;
    [SerializeField] private EnemyData enemyData;
    [SerializeField] private EnemyCombat enemyCombat;

    // Enemy stats
    [Header("Enemy Base Stats")]
    private float _maxHealth;
    private float _health;
    private int _lvl;

    // Attack stats
    [Header("Attack Stats")]
    private int _damage;
    private int _attackRange;
    private float _attackCooldown;
    private int _criticalChance;
    private int _criticalDamage;
    private int _armorPenetration;
    private float _attackSpeed;
   
    // Defence stats
    [Header("Defence Stats")]
    private int _armor;
    private int _magicResistance;
   
    // Movement stats
    [Header("Movement Stats")]
    private float _movementSpeed;
   
    // Enemy type
    [Header("Enemy Type")]
    private bool _isRanged;
    private bool _isMelee;
    private bool _isMagic;
    private bool _isPhysical;
    
    // Debug
    [Header("Debug")]
    public string enemyStatus;
    public GameObject damagePopup;
    public Animator animator;

    private void Awake()
    {
        SetEnemyVariables();
    }

    private void SetEnemyVariables()
    {
        _enemySprite = enemyData.enemySprite;
        _enemyName = enemyData.enemyName;
        _enemyProjectile = enemyData.enemyProjectile;
        _maxHealth = enemyData.health;
        _health = enemyData.health;
        _lvl = enemyData.lvl;
        _damage = enemyData.damage;
        _attackRange = enemyData.attackRange;
        _attackCooldown = enemyData.attackCooldown;
        _criticalChance = enemyData.criticalChance;
        _criticalDamage = enemyData.criticalDamage;
        _armorPenetration = enemyData.armorPenetration;
        _attackSpeed = enemyData.attackSpeed;
        _armor = enemyData.armor;
        _magicResistance = enemyData.magicResistance;
        _movementSpeed = enemyData.movementSpeed;
        _isRanged = enemyData.isRanged;
        _isMelee = enemyData.isMelee;
        _isMagic = enemyData.isMagic;
        _isPhysical = enemyData.isPhysical;
    }
    
    // Setters Getters Functions
    public Sprite Sprite
    {
        get => _enemySprite;
        set => _enemySprite = value;
    }
    
    public string EnemyName
    {
        get => _enemyName;
        set => _enemyName = value;
    }
    
    public GameObject EnemyProjectile
    {
        get => _enemyProjectile;
        set => _enemyProjectile = value;
    }

    public EnemyCombat EnemyCombat
    {
        get => enemyCombat;
        set => enemyCombat = value;
    }

    // Enemy stats
    public float MaxHealth
    {
        get => _maxHealth;
        set => _maxHealth = value;
    }
    
    public float Health
    {
        get => _health;
        set => _health = value;
    }
    
    public int Lvl
    {
        get => _lvl;
        set => _lvl = value;
    }
    
    // Attack stats
    
    public int Damage
    {
        get => _damage;
        set => _damage = value;
    }
    
    public int AttackRange
    {
        get => _attackRange;
        set => _attackRange = value;
    }
    
    public int CriticalChance
    {
        get => _criticalChance;
        set => _criticalChance = value;
    }
    
    public int CriticalDamage
    {
        get => _criticalDamage;
        set => _criticalDamage = value;
    }
    
    public int ArmorPenetration
    {
        get => _armorPenetration;
        set => _armorPenetration = value;
    }
    
    public float AttackSpeed
    {
        get => _attackSpeed;
        set => _attackSpeed = value;
    }

    public float AttackCooldown
    {
        get => _attackCooldown;
        set => _attackCooldown = value;
    }
    
    // Defence stats
    
    public int Armor
    {
        get => _armor;
        set => _armor = value;
    }
    
    public int MagicResistance
    {
        get => _magicResistance;
        set => _magicResistance = value;
    }
    
    // Movement stats
    
    public float MovementSpeed
    {
        get => _movementSpeed;
        set => _movementSpeed = value;
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
