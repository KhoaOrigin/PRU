using System;
using UnityEngine;

public class Level1EnergyLevel1Enemy : Level1Enemy
{
    [SerializeField] private GameObject energyObject;

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
                Level1Player.TakeDamage(enterDamage);
            }
        }
    }

    protected override void Die()
    {
        if (energyObject != null)
        {
            GameObject energy = Instantiate(energyObject, transform.position, Quaternion.identity);
            Destroy(energy, 5f);
        }
        base.Die();
    }
}
