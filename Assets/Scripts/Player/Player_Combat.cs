using System.Collections.Generic;
using UnityEngine;

public class Player_Combat : MonoBehaviour
{
    // Properties
    public bool isHolding { get; private set; }
    public bool isAttacking { get; private set; }

    // Player Reference
    private Player_Behaviour _playerBehaviour;
    

    // Arrow Variables
    [Header("Arrow Variables")]
    [SerializeField] private GameObject arrowPrefab;
    [SerializeField] private Projectile_Data arrowData;
    
    // Bow Stats
    [Header("Bow Stats")]
    [SerializeField] private Transform bowOutTransform;
    [SerializeField] private float minHoldTime = 0.2f;
    [SerializeField] private float cooldown = 0.2f;

    private float _heldTime;
    private float _currentCooldown;


    private void Start()
    {
        _playerBehaviour = GetComponent<Player_Behaviour>();
    }

    public void Update()
    {
    // Reduce cooldown time
    if (_currentCooldown < cooldown)
    {
        _currentCooldown += Time.deltaTime;
    }

    // Check for attack input and update held time
    if (Input.GetKey(KeyCode.Mouse0) && _currentCooldown >= cooldown)
    {
        isAttacking = true;
        isHolding = true;
        _heldTime += Time.deltaTime;
        _heldTime = Mathf.Min(_heldTime, arrowData.lifeTime);
    }

    // Check for attack release and perform attack if held time is positive
    if (Input.GetKeyUp(KeyCode.Mouse0))
    {
        if (_heldTime > 0)
        {
            Attack();
            _playerBehaviour.playerUi.animateMainSpellIcon();
            _currentCooldown = 0f;
        }
        isAttacking = false;
        isHolding = false;
        _heldTime = 0;
    }
    }

    private void Attack()
    {
        var playerTranform = transform;
        var up = playerTranform.up;
        var arrow = Instantiate(arrowPrefab, bowOutTransform.position, Quaternion.identity);
        var projectileBehaviour = arrow.GetComponent<Projectile_Behaviour>();
        projectileBehaviour.PlayerBehaviour = _playerBehaviour;
        projectileBehaviour.IsCritical = _playerBehaviour.CriticalChance > Random.Range(0, 100);
        projectileBehaviour.Duration = (_heldTime + minHoldTime) * 1.2f;
        // set direction to mouse position
        projectileBehaviour.SetDirection((Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position).normalized);
    }

    public List<object> CalculateDamage(Projectile_Data arrow, int enemyPierce, float holdTime, bool isCriticalHit)
    {
        List<object> damageInfo = new List<object>();
        float arrowDamage = arrow.damage; 
        bool[] arrowEffects = new bool[5];                                                          
        arrowEffects[0] = arrow.isPoisonous;
        arrowEffects[1] = arrow.isBurning;
        arrowEffects[2] = arrow.isFreezing;
        arrowEffects[4] = arrow.isElectrifying;


        float calculatedDamage = (((arrowDamage * (1 + holdTime)) * _playerBehaviour.Damage * 0.2f) / (1 + (enemyPierce * 0.4f)));

        if (isCriticalHit)
        {
            calculatedDamage *= (1 + (_playerBehaviour.CriticalDamage * 0.01f));
        }

        damageInfo.Add(calculatedDamage);
        damageInfo.Add(isCriticalHit);
        damageInfo.Add(arrowEffects);
        
        return damageInfo;
    }

    public float CurrentCooldown
    {
        get => _currentCooldown;
        set => _currentCooldown = value;
    }

    public float MaxCooldown => cooldown;

    public float HeldTime
    {
        get => _heldTime;
        set => _heldTime = value;
    }

    public float HoldTime => arrowData.lifeTime;
}
