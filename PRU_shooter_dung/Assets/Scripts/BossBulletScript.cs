using UnityEngine;

public class BossBulletScript : MonoBehaviour
{
    [SerializeField] public float moveSpeed = 3f;
    [SerializeField] private float timeDestroy = 5f;
    [SerializeField] public float damage = 15f;
    [SerializeField] private GameObject bloodPrefab;
    [SerializeField] private float knockbackForce = 5f;

    void Start()
    {
        Destroy(gameObject, timeDestroy);
    }

    void Update()
    {
        MoveBullet();
    }

    void MoveBullet()
    {
        transform.Translate(Vector2.left * moveSpeed * Time.deltaTime);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            PlayerScript player = collision.GetComponent<PlayerScript>();
            if (player == null)
            {
                Debug.Log("No player found");
                return;
            }

            Vector2 knockbackDirection = (collision.transform.position - transform.position).normalized;
            player.TakeDamage(damage, knockbackDirection * knockbackForce); // Apply knockback

            GameObject blood = Instantiate(bloodPrefab, transform.position, Quaternion.identity);
            Destroy(blood, 1f);

            Destroy(gameObject);
        }
    }
}
