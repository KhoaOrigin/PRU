using UnityEngine;

public class Level4EnergyEnemy : Level4Enemy
{
    [SerializeField] private GameObject energyObject;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            if (player != null)
            {
                player.Takedamage(enterDamage);
            }
        }
    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            if (player != null)
            {
                player.Takedamage(stayDamage);
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
