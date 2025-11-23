using UnityEngine;

public class PlayerCombat : MonoBehaviour
{
    public int baseDamage = 20;
    public float attackRange = 2.2f;
    public float attackCooldown = 0.7f;
    public LayerMask hitMask;
    public Transform attackOrigin;

    float attackTimer = 0f;

    void Update()
    {
        attackTimer -= Time.deltaTime;

        if (Input.GetMouseButtonDown(0) && attackTimer <= 0f) // left click
        {
            Attack();
            attackTimer = attackCooldown;
        }
    }

    void Attack()
    {
        // simple sphere overlap attack
        Collider[] hits = Physics.OverlapSphere(attackOrigin.position, attackRange, hitMask);
        foreach (var c in hits)
        {
            Health enemyHealth = c.GetComponent<Health>();
            if (enemyHealth != null)
            {
                enemyHealth.TakeDamage(baseDamage);
            }
        }
        // add VFX/SFX/Animation triggers here
    }

    void OnDrawGizmosSelected()
    {
        if (attackOrigin)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(attackOrigin.position, attackRange);
        }
    }
}
