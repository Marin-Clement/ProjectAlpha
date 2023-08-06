using System.Collections;
using UnityEngine;

public class Player_Behaviour : MonoBehaviour
{
    // Player Reference

    public Player_Movement playerMovement { get; private set;}

    public Player_Combat playerCombat { get; private set;}

    public Player_UI playerUi { get; private set;}

    public Player_Animation playerAnimation { get; private set;}
    
    public Health playerHealth { get; private set;}

    [SerializeField] public Player_Camera playerCamera;

    [Header("Player Stats")] 

    [SerializeField] private float maxHealth = 100;
    private float health;

    [SerializeField] private int lvl = 1;
    

    // Attack stats
    [Header("Attack Stats")] 
    
    [SerializeField] private int damage = 10;
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
        playerMovement = GetComponent<Player_Movement>();
        playerCombat = GetComponent<Player_Combat>();
        playerUi = GetComponent<Player_UI>();
        playerHealth = GetComponent<Health>();
        playerCamera = GameManager.Instance.playerCamera;
        health = maxHealth;
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

    public int Lvl
    {
        get => lvl;
        set => lvl = value;
    }

    public int Damage
    {
        get => damage;
        set => damage = value;
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

    public int Dodge
    {
        get => dodge;
        set => dodge = value;
    }
}