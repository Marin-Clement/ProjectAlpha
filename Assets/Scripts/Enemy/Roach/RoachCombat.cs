using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoachCombat : EnemyCombat
{
    protected override void Attack()
    {
        if(enemyState == EnemyState.InCooldown) return;
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        Vector3 enemyTransformPos = transform.position;
        Vector3 direction = (player.transform.position - enemyTransformPos).normalized;
        GameObject projectile = Instantiate(enemyProjectile, enemyTransformPos, Quaternion.identity);
        projectile.GetComponent<Projectile_Behaviour>().SetDirection(direction);
        projectile.GetComponent<Projectile_Behaviour>().Duration = 1.5f;
        StartCoroutine(AttackCooldown());
    }

    protected override void SpecialAttack()
    {
        throw new System.NotImplementedException();
    }

    protected override IEnumerator AttackCooldown()
    {
        Debug.Log("Cooldown");
        yield return new WaitForSeconds(_enemyBehaviour.AttackCooldown);
        enemyState = EnemyState.Attack;
    }
}
