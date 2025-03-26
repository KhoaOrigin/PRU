using UnityEngine;

public class Level2Explosion : MonoBehaviour
{
    [SerializeField] private float damage = 25f;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        Level2Player player = collision.GetComponent<Level2Player>();
        Level2Enemy enemy = collision.GetComponent<Level2Enemy>();
        if(collision.CompareTag("Player"))
        {
            player.TakeDamage(damage);
        }
        if (collision.CompareTag("Enemy"))
        {
            player.TakeDamage(damage);
        }
    }
    public void DestroyExplosion()
    {
        Destroy(gameObject);
    }
}
