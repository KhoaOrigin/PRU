using UnityEngine;

public class ExplosioEnemy : Enemy
{
    [SerializeField] private GameObject explosionObject;
    private void CreateExplosion()
    {
        if (explosionObject != null)
        {
            Instantiate(explosionObject, transform.position, Quaternion.identity);
        }
    }
    
    protected override void Die()
    {
        CreateExplosion();
        base.Die();
        
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            CreateExplosion();
            Die();
        }
    }
}
