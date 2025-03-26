using System.Collections;
using UnityEngine;

public class Level5BossBulletSpiralAttack : MonoBehaviour, IAttackLevel5
{
    [SerializeField] private GameObject bullet;
    [SerializeField] private Transform spawnPoint;
    [SerializeField] private int bulletsPerWave = 12;
    [SerializeField] private int waves = 5;
    [SerializeField] private float angleStep = 30f;
    [SerializeField] private float delayBetweenWaves = 0.1f;

    void Start()
    {
        
    }

    private IEnumerator FireSpiral()
    {
        float currentAngle = 0;

        for (int wave = 0; wave < waves; wave++)
        {
            for (int i = 0; i < bulletsPerWave; i++)
            {
                float angle = currentAngle + i * angleStep;
                Quaternion rotation = Quaternion.Euler(0, 0, angle);
                Instantiate(bullet, spawnPoint.position, rotation);
            }

            currentAngle += angleStep / 2;
            yield return new WaitForSeconds(delayBetweenWaves);

        }
    }

    public float ExecuteAttack()
    {
        StartCoroutine(FireSpiral());
        return waves * delayBetweenWaves + 1f;
    }
}
