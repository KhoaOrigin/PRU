using System.Collections;
using UnityEngine;

public class LaserBeam : MonoBehaviour
{
    public Transform laserStart;  // Head of the laser
    public Transform laserMiddle; // Middle part that stretches
    public Transform laserEnd;    // End part
    public GameObject bloodPrefab;
    public GameObject warningLinePrefab; // Prefab for warning line
    private GameObject warningLineTop;
    private GameObject warningLineBottom;

    public float maxLaserLength = 5f;   // Full stretch length
    public float growthSpeed = 5f;      // Stretching speed
    public float widthGrowthSpeed = 3f; // Widening speed
    public float warningTime = 1.5f;    // Time before laser fires
    public float lastingTime = 3.0f;    // Time the beam remains fully expanded
    public float damage = 15f;
    public float knockbackForce = 5f; // Knockback strength
    public float startWidth = 0.5f;  // Thin starting width
    public float finalWidth = 2.0f;  // Full powerful width

    private float currentLength = 0f;
    private SpriteRenderer middleRenderer, endRenderer;
    private BoxCollider2D laserCollider;
    [SerializeField] private AudioClip laserSound; // Assign in Inspector
    private AudioSource audioSource;

#pragma warning disable IDE0052 // Remove unread private members
    private bool isFiring = false;
#pragma warning restore IDE0052 // Remove unread private members


    void Awake()
    {
        // Reset all parts to local zero position
        if (laserStart) laserStart.localPosition = Vector3.zero;
        if (laserMiddle) laserMiddle.localPosition = Vector3.zero;
        if (laserEnd) laserEnd.localPosition = Vector3.zero;
    }

    public void Setup(float maxLength, float growth, float widthSpeed, float warn, float duration, float startW, float finalW, float dmg, float knockbackStr)
    {
        maxLaserLength = maxLength;
        growthSpeed = growth;
        widthGrowthSpeed = widthSpeed;
        warningTime = warn;
        lastingTime = duration;
        startWidth = startW;
        finalWidth = finalW;
        damage = dmg;
        knockbackForce = knockbackStr;

    }



    void Start()
    {
        middleRenderer = laserMiddle.GetComponent<SpriteRenderer>();
        endRenderer = laserEnd.GetComponent<SpriteRenderer>();
        laserCollider = laserMiddle.GetComponent<BoxCollider2D>();

        if (laserCollider == null)
        {
            laserCollider = laserMiddle.gameObject.AddComponent<BoxCollider2D>();
        }
        laserCollider.size = new Vector2(0, startWidth);

        // Head part starts at full width, others start thin
        laserStart.localScale = new Vector3(finalWidth, finalWidth, 1);
        SetLaserWidth(startWidth, false); // Middle & End start thin

        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }
        audioSource.clip = laserSound;
        audioSource.loop = true; // Loop sound during active phases
        audioSource.playOnAwake = false;

        StartCoroutine(PrepareLaser());
    }

    public IEnumerator PrepareLaser()
    {
        CreateWarningLines();

        yield return StartCoroutine(WarningEffect());
        // Hide middle and end parts before firing
        middleRenderer.enabled = false;
        endRenderer.enabled = false;

        DestroyWarningLines();

        // Show middle and end parts
        middleRenderer.enabled = true;
        endRenderer.enabled = true;

        // Phase 1: Stretch the beam while staying thin
        yield return StartCoroutine(ExtendLaser());

        // Phase 2: Expand width after full stretch
        yield return StartCoroutine(WidenLaser());

        // Phase 3: Hold the beam at max width
        yield return new WaitForSeconds(lastingTime);

        // Phase 4: Shrink width before disappearing
        yield return StartCoroutine(ShrinkLaser());

        // Phase 5: Retract the beam from left to right
        yield return StartCoroutine(RetractLaser());

        // Destroy the laser object
        Destroy(gameObject);
    }

    IEnumerator ExtendLaser()
    {
        isFiring = true;
        float acceleration = 1.5f;  // Controls smooth expansion

        if (audioSource != null && laserSound != null)
        {
            audioSource.Play();
        }

        while (currentLength < maxLaserLength)
        {
            currentLength += Time.deltaTime * growthSpeed * acceleration;
            acceleration += 0.5f; // Gradually increase speed

            // Apply stretch in length, keeping thin width for now
            laserMiddle.localScale = new Vector3(-currentLength, startWidth, 1);
            laserMiddle.localPosition = laserStart.localPosition + new Vector3(-currentLength / 2, 0, 0);
            laserEnd.localPosition = laserMiddle.localPosition + new Vector3(-currentLength / 2, 0, 0);

            laserCollider.size = new Vector2(currentLength * 2f, startWidth);
            laserCollider.offset = new Vector2(-currentLength / 2, 0);

            yield return null;
        }
    }

    IEnumerator WidenLaser()
    {
        float currentWidth = startWidth;

        while (currentWidth < finalWidth)
        {
            currentWidth += Time.deltaTime * widthGrowthSpeed;
            SetLaserWidth(currentWidth, true);
            yield return null;
        }
    }

    IEnumerator ShrinkLaser()
    {
        float currentWidth = finalWidth;

        while (currentWidth > startWidth)
        {
            currentWidth -= Time.deltaTime * widthGrowthSpeed;
            SetLaserWidth(currentWidth, true);
            yield return null;
        }

        if (audioSource != null && audioSource.isPlaying)
        {
            audioSource.Stop();
        }
    }

    IEnumerator RetractLaser()
    {
        float retractSpeed = growthSpeed * 6f; // Slightly slower than extend
        while (currentLength > 0)
        {
            currentLength -= Time.deltaTime * retractSpeed;
            if (currentLength < 0) currentLength = 0;

            laserMiddle.localScale = new Vector3(-currentLength, startWidth, 1);
            laserMiddle.localPosition = laserStart.localPosition + new Vector3(-currentLength / 2, 0, 0);
            laserEnd.localPosition = laserMiddle.localPosition + new Vector3(-currentLength / 2, 0, 0);

            yield return null;
        }
    }

    void SetLaserWidth(float width, bool includeHead)
    {
        if (includeHead)
            laserStart.localScale = new Vector3(width, width, 1); // Widen head too

        laserMiddle.localScale = new Vector3(laserMiddle.localScale.x, width, 1);
        laserEnd.localScale = new Vector3(width, width, 1);
    }

    void CreateWarningLines()
    {
        if (warningLinePrefab != null)
        {
            warningLineTop = Instantiate(warningLinePrefab, laserStart.position, Quaternion.identity);
            warningLineBottom = Instantiate(warningLinePrefab, laserStart.position, Quaternion.identity);

            UpdateWarningLines();
        }
    }

    void UpdateWarningLines()
    {
        if (warningLineTop == null || warningLineBottom == null) return;

        float halfWidth = finalWidth / 2f;

        // Calculate positions
        Vector3 startPos = laserStart.position;
        Vector3 endPos = laserStart.position + -laserStart.right * maxLaserLength;

        // Offset the lines to match laser width
        Vector3 offset = laserStart.up * halfWidth;

        // Set positions
        warningLineTop.transform.position = (startPos + endPos) / 2 + offset;
        warningLineBottom.transform.position = (startPos + endPos) / 2 - offset;

        // Scale lines to match laser length
        float distance = Vector2.Distance(startPos, endPos);
        warningLineTop.transform.localScale = new Vector3(distance, 0.1f, 1f);
        warningLineBottom.transform.localScale = new Vector3(distance, 0.1f, 1f);

        // Rotate them properly
        float angle = Mathf.Atan2(endPos.y - startPos.y, endPos.x - startPos.x) * Mathf.Rad2Deg;
        warningLineTop.transform.rotation = Quaternion.Euler(0, 0, angle);
        warningLineBottom.transform.rotation = Quaternion.Euler(0, 0, angle);
    }

    void DestroyWarningLines()
    {
        if (warningLineTop != null) Destroy(warningLineTop);
        if (warningLineBottom != null) Destroy(warningLineBottom);
    }

    IEnumerator WarningEffect()
    {
        float flashSpeed = 0.2f;
        for (float t = 0; t < warningTime; t += flashSpeed)
        {
            warningLineTop.SetActive(!warningLineTop.activeSelf);
            warningLineBottom.SetActive(!warningLineBottom.activeSelf);
            yield return new WaitForSeconds(flashSpeed);
        }
    }

}
