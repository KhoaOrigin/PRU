using UnityEngine;
using UnityEngine.Rendering;

public class Level4Explosion : MonoBehaviour
{
    [SerializeField] private float damage = 25f;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Level4Player player = collision.GetComponent<Level4Player>();
        Level4Enemy enemy = collision.GetComponent<Level4Enemy>();

        if (collision.CompareTag("Player"))
        {
            player.Takedamage(damage);
        }

        if (collision.CompareTag("Enemy"))
        {
            enemy.Takedamage(damage);
        }
    }

    public void DestroyExplosion()
    {
        Destroy(gameObject);
    }
}
