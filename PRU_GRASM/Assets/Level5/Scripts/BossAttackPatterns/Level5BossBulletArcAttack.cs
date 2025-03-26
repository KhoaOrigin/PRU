using System.Collections;
using UnityEngine;

public class Level5BossBulletArcAttack : MonoBehaviour, IAttackLevel5
{
    [SerializeField] private GameObject bullet;
    [SerializeField] private Transform spawnPoint;
    [SerializeField] private int bulletCount = 8;
    [SerializeField] private float startAngle = -50f, endAngle = 50f;
    [SerializeField] private float shotDelay = 0.2f;

    void Start()
    {
        
    }

    private IEnumerator FireArc()
    {
        float angleStep = (endAngle - startAngle) / (bulletCount - 1);

        for (int i = 0; i < bulletCount; i++)
        {
            float angle = startAngle + i * angleStep;
            Quaternion rotation = Quaternion.Euler(0, 0, angle);
            Instantiate(bullet, spawnPoint.position, rotation);
        }

        yield return new WaitForSeconds(shotDelay);
    }

    public float ExecuteAttack()
    {
        StartCoroutine(FireArc());
        return 3f;
    }
}
