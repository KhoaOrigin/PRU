using System.Collections;
using UnityEngine;

public class Level5BossBulletExpandingRingAttack : MonoBehaviour, IAttackLevel5
{
    [SerializeField] private GameObject bullet;
    [SerializeField] private Transform spawnPoint;
    [SerializeField] private int bulletsPerRing = 10;
    [SerializeField] private int rings = 3;
    [SerializeField] private float ringDelay = 0.3f;
    [SerializeField] private float expansionFactor = 1.5f;

    void Start()
    {
        
    }

    private IEnumerator FireExpandingRings()
    {
        float radius = 1f;

        for (int i = 0; i < rings; i++)
        {
            for (int j = 0; j < bulletsPerRing; j++)
            {
                float angle = (j * 360f) / bulletsPerRing;
                Vector3 spawnPos = spawnPoint.position + new Vector3(Mathf.Cos(angle), Mathf.Sin(angle), 0) * radius;
                Instantiate(bullet, spawnPos, Quaternion.Euler(0, 0, angle));
            }

            radius *= expansionFactor;
            yield return new WaitForSeconds(ringDelay);
        }
    }

    public float ExecuteAttack()
    {
        StartCoroutine(FireExpandingRings());
        return 3f;
    }
}
