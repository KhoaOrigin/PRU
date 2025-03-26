using UnityEngine;
using System.Collections;

public class Level5EnemyWeaponScript : MonoBehaviour
{
    private float rotateOffset = 180f;
    [SerializeField] private Transform firePos;
    [SerializeField] private GameObject laserPrefab;
    [SerializeField] private GameObject warningLinePrefab;

    private Transform player;
    private Vector3 originalScale;
    private Vector3 lockedTarget; // Stores the last locked position
    private bool isLocked = false; // Flag to stop movement when locked
    private GameObject warningLineInstance;
    private Coroutine aimCoroutine;


    [SerializeField] private float aimTime = 2f;  // Time spent following the player
    [SerializeField] private float warningTime = 0.25f;
    [SerializeField] private float lastingTime = 1f;
    [SerializeField] private float damage = 15f;
    [SerializeField] private float knockback = 5f;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player")?.transform;
        originalScale = transform.localScale;


        if (player != null)
        {
            aimCoroutine = StartCoroutine(AimLockAndFireLoop());
        }
    }

    private IEnumerator AimLockAndFireLoop()
    {
        while (true)
        {
            isLocked = false;
            yield return StartCoroutine(AimAndLockPlayer());

            isLocked = true;

            FireLaser();
            DestroyWarningLine();  // Remove warning line
            yield return new WaitForSeconds(lastingTime + aimTime);
        }
    }

    private IEnumerator AimAndLockPlayer()
    {
        float elapsedTime = 0f;

        while (elapsedTime < aimTime)
        {
            RotateGunToPlayer();
            lockedTarget = player.position;
            CreateOrUpdateWarningLine();
            elapsedTime += Time.deltaTime;
            yield return null;
        }
    }

    private void FireLaser()
    {
        GameObject laser = Instantiate(laserPrefab, firePos.position, firePos.rotation);
        Level5LaserBeam laserScript = laser.GetComponent<Level5LaserBeam>();

        laserScript.Setup(26f, 50f, 50f, warningTime, lastingTime, 0.5f, 1.25f, damage, knockback);
    }

    void Update()
    {
        if (!isLocked) RotateGunToPlayer();
        if (warningLineInstance != null) CreateOrUpdateWarningLine();
    }

    void RotateGunToPlayer()
    {
        if (player == null) return;

        Vector3 displacement = transform.position - player.position;
        float angle = Mathf.Atan2(displacement.y, displacement.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, angle + rotateOffset);

        // Flip gun properly
        if (angle < -90 || angle > 90)
        {
            transform.localScale = new Vector3(originalScale.x, originalScale.y, originalScale.z);
        }
        else
        {
            transform.localScale = new Vector3(originalScale.x, -originalScale.y, originalScale.z);
        }
    }
    

    void CreateOrUpdateWarningLine()
    {
        if (warningLineInstance == null && warningLinePrefab != null)
        {
            warningLineInstance = Instantiate(warningLinePrefab, firePos.position, Quaternion.identity);
        }

        if (warningLineInstance != null && player != null)
        {
            Vector3 startPos = firePos.position;
            Vector3 endPos = isLocked ? lockedTarget : player.position;

            // Position warning line at center between firePos and target
            warningLineInstance.transform.position = (startPos + endPos) / 2;

            // Scale the line based on distance
            float distance = Vector2.Distance(startPos, endPos);
            warningLineInstance.transform.localScale = new Vector3(distance, 0.1f, 1f);

            // Rotate to face target
            Vector3 direction = endPos - startPos;
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            warningLineInstance.transform.rotation = Quaternion.Euler(0, 0, angle);
        }
    }

    void DestroyWarningLine()
    {
        if (warningLineInstance != null)
        {
            Debug.Log($"Destroying warning line instance: {warningLineInstance.name}");
            Destroy(warningLineInstance);
            warningLineInstance = null;
        }
    }

    public void OnEnemyDeath()
    {
        Debug.Log($"{name} (Level5EnemyWeaponScript) handling OnEnemyDeath");
        if (aimCoroutine != null)
        {
            StopCoroutine(aimCoroutine);
            aimCoroutine = null;
            Debug.Log("Stopped aim coroutine");
        }
        DestroyWarningLine();
    }
}
