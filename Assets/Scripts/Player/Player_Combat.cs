using System.Collections.Generic;
using UnityEngine;

public class Player_Combat : MonoBehaviour
{
    // Properties
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
            _heldTime = 0;
        }
    }

    private void Attack()
    {
        var arrow = Instantiate(arrowPrefab, bowOutTransform.position, Quaternion.identity);
        var projectileBehaviour = arrow.GetComponent<Projectile_Behaviour>();;
        projectileBehaviour.IsCritical = _playerBehaviour.CriticalChance > Random.Range(0, 100);
        projectileBehaviour.Damage = CalculateDamage(arrowData, projectileBehaviour.IsCritical);
        projectileBehaviour.Duration = (_heldTime + minHoldTime) * 1.2f;
        // set direction to mouse position
        projectileBehaviour.SetDirection((Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position));
    }

    private List<object> CalculateDamage(Projectile_Data arrow, bool isCriticalHit)
    {
        // * 0 = Damage, 1 = IsCritical, 3 = ArrowEffects

        float calculatedDamage = _playerBehaviour.Damage * 0.5f * arrow.damage * 0.5f / (1 + (-_heldTime)) *
                                 _playerBehaviour.Lvl * 0.5f;

        if (isCriticalHit)
        {
            calculatedDamage *= (1 + (_playerBehaviour.CriticalDamage * 0.025f));
        }

        List<object> damageInfo = new List<object>();

        bool[] arrowEffects = new bool[5];                                                          
        arrowEffects[0] = arrow.isPoisonous;
        arrowEffects[1] = arrow.isBurning;
        arrowEffects[2] = arrow.isFreezing;
        arrowEffects[4] = arrow.isElectrifying;

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
