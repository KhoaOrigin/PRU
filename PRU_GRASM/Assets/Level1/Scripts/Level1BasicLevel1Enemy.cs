using System;
using UnityEngine;

public class Level1BasicLevel1Enemy : Level1Enemy
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            if (Level1Player != null)
            {
                Level1Player.TakeDamage(enterDamage);
            }
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            if (Level1Player != null)
            {
                Level1Player.TakeDamage(stayDamage);
            }
        }
    }
}
