using System.Collections;
using UnityEngine;

public class Level5BossRotatingLaser : MonoBehaviour, IAttackLevel5
{
    [SerializeField] private GameObject laserPrefab;
    [SerializeField] private Transform spawnPoint;
    [SerializeField] private int laserCount = 6;
    [SerializeField] private float rotationSpeed = 50f;
    [SerializeField] private float startRadius = 1f;
    [SerializeField] private float rotationDuration = 5f; // How long to rotate before cleanup

    private Transform[] laserTransforms;
    private Level5LaserBeam laserBeam;
    private bool isRotating = true;

    void Awake()
    {
        laserBeam = GetComponent<Level5LaserBeam>();
    }

    void Start()
    {

        
    }

    private IEnumerator RotatingLaserPattern()
    {
        // Spawn all lasers
        laserTransforms = new Transform[laserCount];
        float angleStep = 360f / laserCount;

        for (int i = 0; i < laserCount; i++)
        {
            float angle = i * angleStep;
            Vector3 offset = new Vector3(Mathf.Cos(angle * Mathf.Deg2Rad), Mathf.Sin(angle * Mathf.Deg2Rad), 0) * startRadius;
            Vector3 spawnPosition = spawnPoint.position + offset;
            Quaternion rotation = Quaternion.Euler(0, 0, angle);
            GameObject laser = Instantiate(laserPrefab, spawnPosition, rotation);
            laserTransforms[i] = laser.transform;
            laser.transform.SetParent(spawnPoint);
        }

        // Rotate for the specified duration
        float elapsedTime = 0f;
        while (elapsedTime < rotationDuration && isRotating)
        {
            spawnPoint.Rotate(0, 0, rotationSpeed * Time.deltaTime);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

    }

    public float ExecuteAttack()
    {
        StartCoroutine(RotatingLaserPattern());
        return rotationDuration + 1f;
    }
}