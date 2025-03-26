using System.Collections;
using UnityEngine;

public class HeaEnemy : Enemy
{
    [SerializeField] private float healValue = 10f;
    protected override void Die()
    {
        base.Die();
        HealPlayer();
    }
    private void HealPlayer()
    {
        if(player != null)
        {
            player.heal(healValue);
        }
    }
}
