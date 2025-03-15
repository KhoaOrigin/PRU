using System.Collections;
using UnityEngine;

public class BossBulletRadialAttack : MonoBehaviour
{
    [SerializeField] private GameObject bullet;
    [SerializeField] private Transform spawnPoint;
    [SerializeField] private int bulletsPerWave = 16;
    [SerializeField] private float delayBetweenWaves = 0.5f;

    void Start()
    {
        StartCoroutine(FireRadialBurst());
    }

    private IEnumerator FireRadialBurst()
    {
        float angleStep = 360f / bulletsPerWave;


        for (int i = 0; i < bulletsPerWave; i++)
        {
            float angle = i * angleStep;
            Quaternion rotation = Quaternion.Euler(0, 0, angle);
            Instantiate(bullet, spawnPoint.position, rotation);
        }

        yield return new WaitForSeconds(delayBetweenWaves);
    }
}
