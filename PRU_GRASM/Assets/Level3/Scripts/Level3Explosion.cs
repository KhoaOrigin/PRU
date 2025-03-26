using UnityEngine;

public class Explosion : MonoBehaviour
{
    [SerializeField] private float explosionDame = 25f;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        Player player = collision.GetComponent<Player>();
        Enemy enemy = collision.GetComponent<Enemy>(); 
        if (collision.CompareTag("Enemy"))
        {
            enemy.TakeDamage(explosionDame);
        }
        if (collision.CompareTag("Player"))
        {
            player.TakeDame(explosionDame);
        }
    }
    public void DestroyExplosion()
    {
        Destroy(gameObject);
    }
}
