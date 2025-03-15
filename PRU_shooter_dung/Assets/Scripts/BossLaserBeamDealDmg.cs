using UnityEngine;

public class BossLaserBeamDealDmg : MonoBehaviour
{
    private LaserBeam laserBeam;

    void Awake()
    {
        laserBeam = GetComponentInParent<LaserBeam>(); // Find LaserBeam from the parent
    }


    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            PlayerScript player = collision.GetComponent<PlayerScript>();
            if (player == null)
            {
                return;
            }

            Vector2 laserDirection = transform.right.normalized; // Laser's direction
            Vector2 playerDirection = (collision.transform.position - transform.position).normalized;

            // Calculate perpendicular direction (left or right)
            Vector2 knockbackDirection = Vector2.Perpendicular(laserDirection);

            // Decide knockback side based on player's position
            if (Vector2.Dot(playerDirection, knockbackDirection) < 0)
            {
                knockbackDirection = -knockbackDirection;
            }

            player.TakeDamage(laserBeam.damage, knockbackDirection * laserBeam.knockbackForce);


            GameObject blood = Instantiate(laserBeam.bloodPrefab, transform.position, Quaternion.identity);
            Destroy(blood, 1f);
        }
    }
}
