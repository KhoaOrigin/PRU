using UnityEngine;

public class Level4BossEnemy : Level4Enemy
{
    [SerializeField] private GameObject bulletPrefabs;
    [SerializeField] private Transform firePoint;
    [SerializeField] private float speedDanThuong = 20f;
    [SerializeField] private float speedDanVongTron = 10f;
    [SerializeField] private float hpValue = 100f;
    [SerializeField] private GameObject miniEnemy;
    [SerializeField] private float skillCooldown = 2f;
    [SerializeField] private GameObject usbPrefabs;

    private float nextSkillTime = 0f;

    protected override void Update()
    {
        base.Update();
        if (Time.time >= nextSkillTime)
        {
            UseRandomSkill();
        }
    }

    protected override void Die()
    {
        Instantiate(usbPrefabs, transform.position, Quaternion.identity);
        base.Die();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            if (player != null)
            {
                player.Takedamage(enterDamage);
            }
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            if (player != null)
            {
                player.Takedamage(stayDamage);
            }
        }
    }

    private void BanDanThuong()
    {
        if (player != null)
        {
            Vector3 directionToPlayer = player.transform.position - firePoint.position;
            directionToPlayer.Normalize();
            GameObject bullet = Instantiate(bulletPrefabs, firePoint.position, Quaternion.identity);
            Level4EnemyBullet enemyBullet = bullet.AddComponent<Level4EnemyBullet>();
            enemyBullet.setMovementDirection(directionToPlayer *  speedDanThuong);
        }
    }

    private void BanDanVongTron()
    {
        const int bulletCount = 12;
        float angleStep = 360f / bulletCount;
        for (int i = 0; i < bulletCount; i++)
        {
            float angle = i * angleStep;
            Vector3 bulletDirection = new Vector3(Mathf.Cos(Mathf.Deg2Rad * angle), Mathf.Sin(Mathf.Deg2Rad * angle), 0);
            GameObject bullet = Instantiate(bulletPrefabs, transform.position, Quaternion.identity);
            Level4EnemyBullet enemyBullet = bullet.AddComponent<Level4EnemyBullet>();
            enemyBullet.setMovementDirection(bulletDirection * speedDanVongTron);
        }
    }

    private void HoiMau(float hpAmount)
    {
        currentHp = Mathf.Min(currentHp + hpAmount, maxHp);
        UpdateHpBar();
    }

    private void SinhMiniEnemy()
    {
        Instantiate(miniEnemy, transform.position, Quaternion.identity);
    }

    private void DiChuyen()
    {
        if (player != null)
        {
            transform.position = player.transform.position;
        }
    }

    private void RandomSkill()
    {
        int randomSkill = Random.Range(0, 5);
        switch (randomSkill)
        {
            case 0:
                BanDanThuong();
                break;

            case 1:
                BanDanVongTron();
                break;

            case 2:
                HoiMau(hpValue);
                break;

            case 3:
                SinhMiniEnemy();
                break;

            case 4:
                DiChuyen();
                break;
        }
    }

    private void UseRandomSkill()
    {
        nextSkillTime = Time.time + skillCooldown;
        RandomSkill();
    }
}
