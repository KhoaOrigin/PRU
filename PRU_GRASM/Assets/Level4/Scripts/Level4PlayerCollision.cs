using UnityEngine;

public class Level4PlayerCollision : MonoBehaviour
{
    [SerializeField] private Level4GameManager gameManager;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("EnemyBullet"))
        {
            Level4Player player = GetComponent<Level4Player>();
            player.Takedamage(10f);
        }
        else if (collision.CompareTag("Usb"))
        {
            gameManager.WinGame();
            Destroy(collision.gameObject);
        }
        else if (collision.CompareTag("Energy"))
        {
            gameManager.AddEnergy();
            Destroy(collision.gameObject);
        }
    }
}
