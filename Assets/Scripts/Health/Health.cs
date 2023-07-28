using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{
    
    enum ObjectType{ Player, Enemy, Object }

    private ObjectType healthType;

    [Header("Health")]

    [SerializeField] private float maxHealth = 100;

    private float health;

    [Header("Enemy Reference")]
    [SerializeField] private GameObject damagePopup;

    [Header("(DEBUG) Dummy")]
    [SerializeField] private bool isDummy; 
    [SerializeField] private float dummyCooldown = 3f;
    private float dummyCooldownTimer;

    [Header("Status Effects")]

    [SerializeField] private float statusEffectTickRate = 1f;
    private float statusEffectTimer;

    // * Poison
    private bool isPoisoned;
    private int maxPoisonStacks = 5;
    private int poisonStacks;

    // * Burning
    private bool isBurning;
    
    private void Start()
    {
        health = maxHealth;
        this.healthType = gameObject.tag switch
        {
            "Player" => ObjectType.Player,
            "Enemy" => ObjectType.Enemy,
            "Object" => ObjectType.Object,
            _ => ObjectType.Object
        };
    }

    private void Update()
    {
        // ! This is a temporary solution
        if (isDummy)
        {
            if (dummyCooldownTimer > 0)
            {
                dummyCooldownTimer -= Time.deltaTime;
            }
            else
            {
                dummyCooldownTimer = dummyCooldown;
                health = maxHealth;
            }
        }
        // !
    }

    public void TakeDamage(List<object> damageInfo)
    {
        float damage = (float) damageInfo[0];
        bool isCritical = (bool) damageInfo[1];

        if (healthType == ObjectType.Player)
        {
            health -= damage;
        }
        else if (healthType == ObjectType.Enemy)
        {
            health -= damage;
            GameObject damagePopupInstance = Instantiate(damagePopup, transform.position, Quaternion.identity);
            DamageFloatingText floatingText = damagePopupInstance.GetComponent<DamageFloatingText>();
            floatingText.Damage = damage;
            floatingText.IsCritical = isCritical;

            // ! This is a temporary solution
            if (isDummy)
            {
                dummyCooldownTimer = dummyCooldown;
            }
            // !
        }
        else if (healthType == ObjectType.Object)
        {
            health -= damage;
        }
        else
        {
            Debug.Log("ObjectType not found");
        }
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (healthType == ObjectType.Player)
        {
            switch (col.gameObject.tag)
            {
                case "Enemy":
                    TakeDamage(new List<object>(){10f, false});
                    GameManager.Instance.playerBehaviour.playerMovement.Knockback(col.gameObject.transform.position);
                    GameManager.Instance.playerBehaviour.playerCamera.ShakeCamera(0.2f, 0.1f);
                    break;
                case "EnemyProjectile":
                    TakeDamage(new List<object>(){col.gameObject.GetComponent<Projectile_Behaviour>().projectileData.damage, false});
                    GameManager.Instance.playerBehaviour.playerCamera.ShakeCamera(0.2f, 0.1f);
                    break;
            }
        }
    }

    public float MaxHealth
    {
        get => maxHealth;
        set => maxHealth = value;
    }

    public float HealthValue
    {
        get => health;
        set => health = value;
    }
}