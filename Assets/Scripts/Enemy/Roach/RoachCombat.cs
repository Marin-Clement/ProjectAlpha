using System.Collections;
using UnityEngine;

public class RoachCombat : EnemyCombat
{
    // * constants
    private readonly float _fireInterval = 0.05f;                        // time between each projectile
    private readonly int _numberOfProjectiles = 16;                      // number of projectiles to fire
    private readonly int _specialChance = 10;                           // chance to use special attack in percent
    private readonly float _offsetDistance = 1.5f;                       // distance from the enemy to the projectile

    // ! live variables
    private float _angle = 0.0f;

    public override void Attack()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if(attackState == EnemyState.InCooldown) return;
        Vector3 enemyTransformPos = transform.position;
        Vector3 direction = (player.transform.position - enemyTransformPos).normalized;
        GameObject projectile = Instantiate(enemyProjectile, enemyTransformPos, Quaternion.identity);
        Projectile_Behaviour projectileBehaviour = projectile.GetComponent<Projectile_Behaviour>();
        projectileBehaviour.SetDirection(direction);
        projectileBehaviour.Duration = 1.5f;
        projectileBehaviour.Damage = CalculateDamage(projectileData);
        StartCoroutine(AttackCooldown());
    }

    public override void SpecialAttack()
    {
        if (isCastingSpecialAttack) return;
        isCastingSpecialAttack = true;
        StartCoroutine(SpecialAttackPreview());
    }

    private IEnumerator SpecialAttackCoroutine()
    {
        for (int i = 0; i < _numberOfProjectiles; i++)
        {
            Vector3 startPosition = transform.position;
            Vector2 offset = new Vector2(Mathf.Cos(_angle), Mathf.Sin(_angle)) * _offsetDistance;
            startPosition += new Vector3(offset.x, offset.y, 0f);

            GameObject projectile = Instantiate(enemyProjectile, startPosition, Quaternion.identity);
            Projectile_Behaviour projectileBehaviour = projectile.GetComponent<Projectile_Behaviour>();
            projectileBehaviour.SetDirection(new Vector3(Mathf.Cos(_angle), Mathf.Sin(_angle), 0.0f));
            projectileBehaviour.Duration = 1.5f;
            projectileBehaviour.Damage = CalculateDamage(projectileData);
            _angle += 2 * Mathf.PI / _numberOfProjectiles;
            yield return new WaitForSeconds(_fireInterval);
        }
        _angle = 0.0f;
        isCastingSpecialAttack = false;
    }

    protected override IEnumerator SpecialAttackPreview()
    {
        LineRenderer lineRenderer = new GameObject("Line").AddComponent<LineRenderer>();
        lineRenderer.material = new Material(Shader.Find("Sprites/Default"));
        lineRenderer.startColor = new Color(1.0f, 0.0f, 0.0f, 0.25f);
        lineRenderer.endColor = new Color(1.0f, 0.0f, 0.0f, 0.10f);
        lineRenderer.startWidth = 0.25f;
        lineRenderer.endWidth = 1f;

        for (int i = 0; i < _numberOfProjectiles; i++)
        {
            Vector3 lineStartPosition = transform.position;
            Vector2 offset = new Vector2(Mathf.Cos(_angle), Mathf.Sin(_angle)) * _offsetDistance;
            lineStartPosition += new Vector3(offset.x, offset.y, 0f);

            RaycastHit2D hit = Physics2D.Raycast(lineStartPosition, new Vector2(Mathf.Cos(_angle), Mathf.Sin(_angle)), 100.0f, LayerMask.GetMask("obstacles"));
            if (hit.collider)
            {
                LineRenderer previewLineRenderer = Instantiate(lineRenderer, transform.position, Quaternion.identity);
                previewLineRenderer.SetPosition(0, lineStartPosition);
                previewLineRenderer.SetPosition(1, hit.point * 1.05f);
                Destroy(previewLineRenderer.gameObject, _enemyBehaviour.AttackCooldown/2);
            }

            yield return new WaitForSeconds(_fireInterval);
            _angle += 2 * Mathf.PI / _numberOfProjectiles;
        }
        _angle = 0.0f;
        Destroy(lineRenderer.gameObject);
        yield return new WaitForSeconds(_enemyBehaviour.AttackCooldown/3);
        StartCoroutine(SpecialAttackCoroutine());
        StartCoroutine(AttackCooldown());
    }



    protected override IEnumerator AttackCooldown()
    {
        _enemyBehaviour.enemyState = EnemyBehaviour.EnemyState.Chase;
        attackState = EnemyState.InCooldown;
        yield return new WaitForSeconds(_enemyBehaviour.AttackCooldown);
        attackState = Random.Range(0, 100) > _specialChance ? EnemyState.Attack : EnemyState.AttackSpecial;
    }
}
