using System.Collections;
using UnityEngine;

public class Level5BossBarrageLaserAttack : MonoBehaviour, IAttackLevel5
{
    [SerializeField] private GameObject laserPrefab; // Laser beam prefab
    [SerializeField] private Transform spawnPoint;   // Base spawn position

    [SerializeField] private float maxLength = 26f;
    [SerializeField] private float growthSpeed = 50f;
    [SerializeField] private float widthSpeed = 50f;
    [SerializeField] private float warningTime = 0.75f;
    [SerializeField] private float lastingTime = 3f;
    [SerializeField] private float startWidth = 0.5f;
    [SerializeField] private float finalWidth = 2f;
    [SerializeField] private float damage = 15f;
    [SerializeField] private float knockback = 5f;

    [SerializeField] private int beamCount = 4; // Number of beams
    [SerializeField] private float verticalSpacing = 3f; // Space between each laser

    private void Start()
    {
       
    }

    private IEnumerator SpawnLaserBarrage()
    {
        for (int i = 0; i < beamCount; i++)
        {
            Vector3 spawnPos = spawnPoint.position + new Vector3(0, -i * verticalSpacing, 0);
            GameObject laser = Instantiate(laserPrefab, spawnPos, spawnPoint.rotation);
            Level5LaserBeam laserScript = laser.GetComponent<Level5LaserBeam>();

            laser.transform.SetParent(spawnPoint);
            laserScript.Setup(maxLength, growthSpeed, widthSpeed, warningTime, lastingTime, startWidth, finalWidth, damage, knockback);

            yield return new WaitForSeconds(lastingTime); // Wait until previous beam disappears
        }
    }

    public float ExecuteAttack()
    {
        StartCoroutine(SpawnLaserBarrage());
        return warningTime + (lastingTime * beamCount);
    }
}
