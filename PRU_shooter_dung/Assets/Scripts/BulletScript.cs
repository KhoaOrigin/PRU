using UnityEngine;

public class BulletScript : MonoBehaviour
{
    [SerializeField] public float moveSpeed = 25f;
    [SerializeField] private float timeDestroy = 0.5f;
    [SerializeField] public float damage = 15f;
    [SerializeField] private GameObject bloodPrefab;

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
        transform.Translate(Vector2.right * moveSpeed *  Time.deltaTime);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy"))
        {
            Enemy enemy = collision.GetComponent<Enemy>();
            if (enemy == null)
            {
                Debug.Log("No enemy");
                return;
            }

            enemy.TakeDamage(damage);
            GameObject blood = Instantiate(bloodPrefab, transform.position, Quaternion.identity);
            Destroy(blood, 1f);

            Destroy(gameObject);
        } else if (collision.CompareTag("Limiter"))
        {
            Destroy(gameObject);
        }
    }
}
