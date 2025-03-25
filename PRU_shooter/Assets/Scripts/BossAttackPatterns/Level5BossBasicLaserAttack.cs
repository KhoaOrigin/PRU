using System.Collections;
using UnityEngine;

public class Level5BossBasicLaserAttack : MonoBehaviour, IAttackLevel5
{
    [SerializeField] private GameObject laserPrefab; // The laser beam prefab
    [SerializeField] private Transform spawnPoint;   // Where the laser spawns

    [SerializeField] private float maxLength = 26f;
    [SerializeField] private float growthSpeed = 50f;
    [SerializeField] private float widthSpeed = 50f;
    [SerializeField] private float warningTime = 0.75f;
    [SerializeField] private float lastingTime = 3f;
    [SerializeField] private float startWidth = 0.5f;
    [SerializeField] private float finalWidth = 2f;
    [SerializeField] private float damage = 15f;
    [SerializeField] private float knockback = 5f;

    private Level5LaserBeam laserBeam;

    void Awake()
    {
        laserBeam = GetComponent<Level5LaserBeam>();
    }

    void Start()
    {
        
    }

    private IEnumerator SpawnLaser()
    {

        GameObject laser = Instantiate(laserPrefab, spawnPoint.position, spawnPoint.rotation);
        Level5LaserBeam laserScript = laser.GetComponent<Level5LaserBeam>();

        laser.transform.SetParent(spawnPoint);
        laserScript.Setup(maxLength, growthSpeed, widthSpeed, warningTime, lastingTime, startWidth, finalWidth, damage, knockback);



        yield return new WaitForSeconds(lastingTime);
    }

    public float ExecuteAttack()
    {
        StartCoroutine(SpawnLaser());
        return warningTime + lastingTime;
    }
}
