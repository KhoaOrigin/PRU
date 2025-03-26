using UnityEngine;

public class Level2BossEnemy : Level2Enemy
{
    [SerializeField] private GameObject bulletPrehabs;
    [SerializeField] private Transform firePoint;
    [SerializeField] private float speedDanThuong = 20f;
    [SerializeField] private float speedDanVongTron = 10f;
    [SerializeField] private float hpValue = 100f;
    [SerializeField] private GameObject miniEnemy;
    [SerializeField] private float skillCoolDown = 2f;
    private float nextSkillTime = 0f;
    [SerializeField] private GameObject usbPrefabs;
    protected override void Update()
    {
        base.Update();
        if(Time.time > nextSkillTime)
        {
            SuDungSkill();
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
                player.TakeDamage(enterDamage);
            }
        }
    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            if (player != null)
            {
                player.TakeDamage(stayDamage);
            }
        }
    }
    private void BanDanThuong()
    {
        if(player != null)
        {
            Vector3 directionToPlayer = player.transform.position - firePoint.position;
            directionToPlayer.Normalize();
            GameObject bullet = Instantiate(bulletPrehabs, firePoint.position, Quaternion.identity);
            Level2EnemyBullet enemyBullet = bullet.AddComponent<Level2EnemyBullet>();
            enemyBullet.SetMoveDirection(directionToPlayer*speedDanThuong);
        }
    }
    private void BanDanVongTron()
    {
        const int bulletCount = 12;
        float angleStep = 360f / bulletCount;
        for (int i = 0; i < bulletCount; i++)
        {
            float angle = i*angleStep;
            Vector3 bulletDirection = new Vector3(Mathf.Cos(Mathf.Deg2Rad * angle), Mathf.Sin(Mathf.Deg2Rad * angle), 0);
            GameObject bullet = Instantiate(bulletPrehabs, transform.position, Quaternion.identity);
            Level2EnemyBullet enemyBullet = bullet.AddComponent<Level2EnemyBullet>();
            enemyBullet.SetMoveDirection(bulletDirection*speedDanVongTron);
        }
    }

    private void HoiMau(float hpAmount)
    {
        currentHp = Mathf.Min(currentHp + hpAmount, maxHp);
        UpdateHpBar();
    }
    private void SinhMiniEnemy()
    {
        Instantiate(miniEnemy, firePoint.position, Quaternion.identity);
    }
    private void DichChuyen()
    {
        if(player !=null)
        {
            transform.position = player.transform.position;
        }
    }

    private void ChonSkillNgauNhien()
    {
        int randomSkill = Random.Range(0, 5);
        switch(randomSkill)
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
                DichChuyen();
                break;
        }
    }
    private void SuDungSkill()
    {
        nextSkillTime = Time.time + skillCoolDown;
        ChonSkillNgauNhien();
    }
}
