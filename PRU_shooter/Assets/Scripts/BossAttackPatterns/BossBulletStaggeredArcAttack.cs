using System.Collections;
using UnityEngine;

public class BossBulletStaggeredArcAttack : MonoBehaviour, IAttack
{
    [SerializeField] private GameObject bullet;
    [SerializeField] private Transform spawnPoint;
    [SerializeField] private int waves = 3;
    [SerializeField] private int bulletsPerWave = 5;
    [SerializeField] private float baseStartAngle = -50f, baseEndAngle = 50f;
    [SerializeField] private float shiftAngle = 5f;
    [SerializeField] private float shotDelay = 0.2f;

    void Start()
    {
        
    }

    private IEnumerator FireStaggeredArc()
    {
        for (int wave = 0; wave < waves; wave++)
        {
            float startAngle = baseStartAngle + wave * shiftAngle;
            float endAngle = baseEndAngle + wave * shiftAngle;
            float angleStep = (endAngle - startAngle) / (bulletsPerWave - 1);

            for (int i = 0; i < bulletsPerWave; i++)
            {
                float angle = startAngle + i * angleStep;
                Quaternion rotation = Quaternion.Euler(0, 0, angle);
                Instantiate(bullet, spawnPoint.position, rotation);
            }

            yield return new WaitForSeconds(shotDelay);

        }

    }

    public float ExecuteAttack()
    {
        StartCoroutine(FireStaggeredArc());
        return waves * shotDelay + 1f;
    }
}
