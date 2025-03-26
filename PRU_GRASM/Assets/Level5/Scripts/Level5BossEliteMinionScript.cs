using UnityEngine;

public class Level5BossEliteMinionScript : Level5Enemy
{
    [SerializeField] private float knockbackForce = 6f;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (player == null) return;

        if (collision.CompareTag("Player"))
        {
            Vector2 knockbackDirection = (collision.transform.position - transform.position).normalized;
            player.TakeDamage(entryDamage, knockbackDirection * knockbackForce);
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
