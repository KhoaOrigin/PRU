using Unity.VisualScripting;
using UnityEngine;

public class BossEnemy : Enemy
{
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private Transform firePoint;
    [SerializeField] private float fireRate = 20f;
    [SerializeField] private float specialAttackRate = 10f;
    [SerializeField] private GameObject miniEnemyPrefab;
    [SerializeField] private float skillCooldown = 10f;
    private float skillTimer = 0f;
    [SerializeField] private GameObject usbPrefabs;

    protected override void Update()
    {
        base.Update();
        if(Time.time >= skillTimer)
        {
            SuDungSkill();
        }
    }
    protected override void Die()
    {
        Instantiate(usbPrefabs, transform.position, Quaternion.identity);
        base.Die();
    }
    private void BanDanThuong()
    {
        if(player != null)
        {
            Vector3 direction = player.transform.position - firePoint.position;
            direction.Normalize();
            GameObject bullet = Instantiate(bulletPrefab, firePoint.position, Quaternion.identity);
            EnemyBullet enemyBullet = bullet.AddComponent<EnemyBullet>();
            enemyBullet.SetMovementDirection(direction * fireRate);
        }
    }
    private void BanDanDacBiet()
    {
        const int bulletcount = 20;
        float angleStep = 360f / bulletcount;
        for (int i = 0; i < bulletcount; i++)
        {
            float angle = i * angleStep;
            Vector3 direction = new Vector3(Mathf.Sin(angle * Mathf.Deg2Rad), Mathf.Cos(angle * Mathf.Deg2Rad), 0);
            GameObject bullet = Instantiate(bulletPrefab, firePoint.position, Quaternion.identity);
            EnemyBullet enemyBullet = bullet.AddComponent<EnemyBullet>();
            enemyBullet.SetMovementDirection(direction * specialAttackRate);
        }
    }
    private void SinhMiniEnemy()
    {
        Instantiate(miniEnemyPrefab, transform.position, Quaternion.identity);
    }
    private void DichChuyen()
    {
        if (player != null)
        {
            transform.position = player.transform.position;
        }
    }
    private void SuDungSKillNgauNhien()
    {
        int random = Random.Range(0, 4);
        switch (random)
        {
            case 0:
                BanDanThuong();
                break;
            case 1:
                BanDanDacBiet();
                break;
            case 2:
                SinhMiniEnemy();
                break;
            case 3:
                DichChuyen();
                break;
        }
    }
    private void SuDungSkill()
    {
        skillTimer = Time.time + skillCooldown;
        SuDungSKillNgauNhien();
    }

    }
