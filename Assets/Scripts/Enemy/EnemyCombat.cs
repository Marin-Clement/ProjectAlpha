using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class EnemyCombat : MonoBehaviour
{
    protected EnemyBehaviour _enemyBehaviour;

    // Prefabs
    [Header("Prefabs")] [SerializeField] protected GameObject enemyProjectile;
    [SerializeField] protected Projectile_Data projectileData;

    //! live variables
    public enum EnemyState
    {
        Attack,
        AttackSpecial,
        InCooldown,
    }

    public EnemyState attackState;

    public bool isCastingSpecialAttack = false;

    //! abstract methods

    public abstract void Attack();
    public abstract void SpecialAttack();

    //! virtual methods
    protected abstract IEnumerator AttackCooldown();
    protected abstract IEnumerator SpecialAttackPreview();

    //! shared methods
    protected virtual void Start()
    {
        _enemyBehaviour = GetComponent<EnemyBehaviour>();
    }

    protected List<object> CalculateDamage(Projectile_Data projectile)
    {
        float calculatedDamage = _enemyBehaviour.Damage * 0.5f * projectile.damage * 0.5f;

        List<object> damageInfo = new List<object>();

        bool[] effects = new bool[5];
        effects[0] = projectile.isPoisonous;
        effects[1] = projectile.isBurning;
        effects[2] = projectile.isFreezing;
        effects[4] = projectile.isElectrifying;

        damageInfo.Add(calculatedDamage); // damage
        damageInfo.Add(false); // isCriticalHit
        damageInfo.Add(effects); // projectileEffects
        return damageInfo;
    }
}