using UnityEngine;

public class Level4ExplosionEnemy : Level4Enemy
{
    [SerializeField] private GameObject explosionPrefabs;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            CreateExplosion();
        }
    }

    protected override void Die()
    {
        CreateExplosion();
        base.Die();
    }

    private void CreateExplosion()
    {
        if (explosionPrefabs != null)
        {
            Instantiate(explosionPrefabs, transform.position, Quaternion.identity);
        }
    }
}
