using UnityEditor;
using UnityEngine;

[CreateAssetMenu(fileName = "New Enemy", menuName = "Enemy/EnemyData")]
public class EnemyData : ScriptableObject
{
   [Header("Enemy Data")]
   public Sprite enemySprite;
   public string enemyName;
   public GameObject enemyProjectile;
   
   // Enemy stats
   [Header("Enemy Base Stats")]
   public float health;
   public int lvl;

   // Attack stats
   [Header("Attack Stats")]
   public int damage;
   public int attackRange;
   public float attackCooldown;
   public int criticalChance;
   public int criticalDamage;
   public int armorPenetration;
   
   // Defence stats
   [Header("Defence Stats")]
   public int armor;
   public int magicResistance;
   
   // Movement stats
   [Header("Movement Stats")]
   public float movementSpeed;
   
   // Enemy type
   [Header("Enemy Type")]
   public bool isRanged;
   public bool isMelee;
   public bool isMagic;
   public bool isPhysical;
}

