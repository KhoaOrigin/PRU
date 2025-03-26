using UnityEngine;

public class Level2PlayerCollision : MonoBehaviour
{
    [SerializeField] private Level2GameManager gameManager;
    [SerializeField] private Level2AudioManager audioManager;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("EnemyBullet"))
        {
            Level2Player player = GetComponent<Level2Player>();
            player.TakeDamage(10f);
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
            audioManager.PlayEnergySound();
        }
    }
}
