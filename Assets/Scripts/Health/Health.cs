using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.Mathf;

public class Health : MonoBehaviour
{
    private enum ObjectType
    {
        Player,
        Enemy,
        Object
    }

    private ObjectType _healthType;

    [Header("Health")] [SerializeField] private float maxHealth = 100;

    private float _health;

    [Header("Enemy Reference")] [SerializeField]
    private GameObject damagePopup;

    [Header("(DEBUG) Dummy")] [SerializeField]
    public bool isDummy;

    [SerializeField] private float dummyCooldown = 3f;
    private float _dummyCooldownTimer;

    [Header("Status Effects")] [SerializeField]
    private float statusEffectTickRate = 1f;

    private float _statusEffectTimer;

    // * Poison
    [SerializeField] public bool isPoisoned;
    [SerializeField] private float maxPoisonDuration = 5f;
    [SerializeField] private float poisonDuration;
    [SerializeField] private int maxPoisonStacks = 5;
    [SerializeField] private int poisonStacks;

    private void Start()
    {
        _health = maxHealth;
        _healthType = gameObject.tag switch
        {
            "Player" => ObjectType.Player,
            "Enemy" => ObjectType.Enemy,
            "Object" => ObjectType.Object,
            _ => ObjectType.Object
        };
        StartCoroutine(StatusEffectTick());
    }

    private void Update()
    {
        // ! This is a temporary solution
        if (!isDummy) return;
        if (_dummyCooldownTimer > 0)
        {
            _dummyCooldownTimer -= Time.deltaTime;
        }
        else
        {
            _dummyCooldownTimer = dummyCooldown;
            _health = maxHealth;
        }
    }

    public void TakeDamage(List<object> damageInfo)
    {
        // * 0 = Damage, 1 = IsCritical, 3 = ArrowEffects
        float damage = (float)damageInfo[0];
        bool arrowisCritical = (bool)damageInfo[1];

        // * (ArrowEffects) 0 = Poison, 1 = Burning, 2 = Freezing, 3 = Electrifying
        bool[] arrowEffects = (bool[])damageInfo[2];
        bool arrowIsPoisoned = arrowEffects[0];
        bool arrowIsBurning = arrowEffects[1];
        bool arrowIsFreezing = arrowEffects[2];
        bool arrowIsElectrifying = arrowEffects[3];

        if (arrowIsPoisoned)
        {
            isPoisoned = true;
            poisonDuration = maxPoisonDuration;
            if (poisonStacks < maxPoisonStacks)
            {
                poisonStacks++;
            }
            else
            {
                poisonStacks = maxPoisonStacks;
            }
        }

        switch (_healthType)
        {
            case ObjectType.Player:
            {
                _health -= RoundToInt(damage);
                GameManager.Instance.player.GetComponent<Player_Behaviour>().playerCamera.ShakeCamera(0.1f, 0.2f);
                GameObject damagePopupInstance = Instantiate(damagePopup, transform.position, Quaternion.identity);
                DamageFloatingText floatingText = damagePopupInstance.GetComponent<DamageFloatingText>();
                floatingText.Damage = damage;
                floatingText.IsCritical = arrowisCritical;
                floatingText.Color = Color.white;
                break;
            }
            case ObjectType.Enemy:
            {
                _health -= RoundToInt(damage);
                GameObject damagePopupInstance = Instantiate(damagePopup, transform.position, Quaternion.identity);
                DamageFloatingText floatingText = damagePopupInstance.GetComponent<DamageFloatingText>();
                floatingText.Damage = damage;
                floatingText.IsCritical = arrowisCritical;
                floatingText.Color = Color.white;

                // ! This is a temporary solution
                if (isDummy)
                {
                    _dummyCooldownTimer = dummyCooldown;
                }

                // !
                break;
            }
            case ObjectType.Object:
                _health -= damage;
                break;
            default:
                Debug.Log("ObjectType not found");
                break;
        }

        // check if dead
        if (!(_health <= 0) || isDummy) return; // Todo: Remove !isDummy
        DropItem dropItem = GetComponent<DropItem>();
        if (dropItem != null)
        {
            dropItem.GenerateItem();
        }

        Destroy(gameObject);
    }

    private void TakeStatusEffectDamage(float damage)
    {
        if (isPoisoned)
        {
            switch (_healthType)
            {
                case ObjectType.Player:
                    _health -= damage * poisonStacks;
                    break;
                case ObjectType.Enemy:
                {
                    _health -= damage * poisonStacks;
                    GameObject damagePopupInstance = Instantiate(damagePopup, transform.position, Quaternion.identity);
                    DamageFloatingText floatingText = damagePopupInstance.GetComponent<DamageFloatingText>();
                    floatingText.Damage = damage * poisonStacks;
                    floatingText.Color = Color.green;

                    // ! This is a temporary solution
                    if (isDummy)
                    {
                        _dummyCooldownTimer = dummyCooldown;
                    }

                    // !
                    break;
                }
                case ObjectType.Object:
                    _health -= damage * poisonStacks;
                    break;
                default:
                    Debug.Log("ObjectType not found");
                    break;
            }
        }

        // check if dead
        if (!(_health <= 0)) return;

        DropItem dropItem = GetComponent<DropItem>();
        if (dropItem != null)
        {
            dropItem.GenerateItem();
        }

        Destroy(gameObject);
    }

    public void Heal(float healAmount)
    {
        _health += healAmount;
        GameObject damagePopupInstance = Instantiate(damagePopup, transform.position, Quaternion.identity);
        DamageFloatingText floatingText = damagePopupInstance.GetComponent<DamageFloatingText>();
        floatingText.Damage = healAmount;
        floatingText.Color = Color.magenta;
        if (_health > maxHealth)
        {
            _health = maxHealth;
        }
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        switch (_healthType)
        {
            case ObjectType.Player:
                switch (col.gameObject.tag)
                {
                    case "Enemy":
                        TakeDamage(new List<object>() { 10f, false, new[] { false, false, false, false } });
                        GameManager.Instance.player.GetComponent<Player_Behaviour>().playerMovement
                            .Knockback(col.gameObject.transform.position);
                        GameManager.Instance.player.GetComponent<Player_Behaviour>().playerCamera
                            .ShakeCamera(0.2f, 0.1f);
                        break;
                    case "EnemyProjectile":
                        TakeDamage(new List<object>()
                        {
                            col.gameObject.GetComponent<Projectile_Behaviour>().projectileData.damage, false,
                            new[] { false, false, false, false }
                        });
                        break;
                }

                break;
            case ObjectType.Enemy:
                switch (col.gameObject.tag)
                {
                    case "PlayerProjectile":
                        TakeDamage(col.gameObject.GetComponent<Projectile_Behaviour>().Damage);
                        GameManager.Instance.player.GetComponent<Player_Behaviour>().playerCamera
                            .ShakeCamera(0.1f, 0.05f);
                        break;
                }

                break;
            case ObjectType.Object:
                switch (col.gameObject.tag)
                {
                    case "PlayerProjectile":
                        TakeDamage(col.gameObject.GetComponent<Projectile_Behaviour>().Damage);
                        break;
                }

                break;
            default:
                Debug.Log("ObjectType not found");
                break;
        }
    }

    // ReSharper disable Unity.PerformanceAnalysis
    private IEnumerator StatusEffectTick()
    {
        while (true)
        {
            if (isPoisoned)
            {
                poisonDuration -= statusEffectTickRate;
                if (poisonDuration <= 0)
                {
                    isPoisoned = false;
                    poisonStacks = 0;
                }
                else
                {
                    TakeStatusEffectDamage(5f);
                }
            }

            yield return new WaitForSeconds(statusEffectTickRate);
        }
    }

    public float MaxHealth
    {
        get => maxHealth;
        set => maxHealth = value;
    }

    public float HealthValue
    {
        get => _health;
        set => _health = value;
    }
}