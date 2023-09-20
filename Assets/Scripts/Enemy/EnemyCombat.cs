using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class EnemyCombat : MonoBehaviour
{
    protected EnemyBehaviour _enemyBehaviour;

    // Prefabs
    [Header("Prefabs")]
    [SerializeField] protected GameObject enemyProjectile;

    //! live variables
    protected enum EnemyState
    {
        Attack,
        AttackSpecial,
        InCooldown,
    }

    protected EnemyState enemyState;

    protected abstract void Attack();
    protected abstract void SpecialAttack();

    protected virtual void Start()
    {
        _enemyBehaviour = GetComponent<EnemyBehaviour>();
    }

    protected void Update()
    {
        if (_enemyBehaviour.enemyState == EnemyBehaviour.EnemyState.Dead) return;
        if (enemyState == EnemyState.Attack)
        {
            Attack();
            enemyState = EnemyState.InCooldown;
        }
        else if (enemyState == EnemyState.AttackSpecial)
        {
            SpecialAttack();
        }
    }

    protected abstract IEnumerator AttackCooldown();

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

        _enemyBehaviour.enemyState = EnemyBehaviour.EnemyState.Chase;
        return damageInfo;
    }
}
