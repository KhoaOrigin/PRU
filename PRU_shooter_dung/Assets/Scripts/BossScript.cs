using UnityEngine;
using UnityEngine.UI;

public class BossScript : Enemy
{
    protected override void Update()
    {
        // Don't call base.Update() since it would trigger MoveToPlayer()
        // Just implement boss-specific behavior
        Attack();
    }

    // Override MoveToPlayer to do nothing (in case it's called from elsewhere)
    protected override void MoveToPlayer()
    {
        return;
    }

    void Attack()
    {
        // Your boss-specific attack logic here
    }

    protected override void Die()
    {
        // Add any special boss death effects/logic here
        base.Die();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (player == null) return;

        if (collision.CompareTag("Player"))
        {
            player.TakeDamage(entryDamage);
        }

    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (player == null) return;

        if (collision.CompareTag("Player"))
        {
            player.TakeDamage(overtimeDamage);
        }
    }
}