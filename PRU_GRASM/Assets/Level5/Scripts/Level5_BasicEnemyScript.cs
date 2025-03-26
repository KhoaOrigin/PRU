using UnityEngine;

public class BasicEnemyScript : Level5Enemy
{
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
