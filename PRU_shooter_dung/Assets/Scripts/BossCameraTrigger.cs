using UnityEngine;
using Unity.Cinemachine;
using System.Collections;

public class BossCameraTrigger : MonoBehaviour
{
    public CinemachineCamera bossCam;  // Assign in Inspector
    private BoxCollider2D sceneLimiterCollider;
    public GameObject bossHPBar; // Assign HP Bar (Canvas) in Inspector
    public float delayBeforeHPBar = 2.0f; // Adjust delay as needed

    private void Start()
    {
        // Make sure Boss Camera is assigned
        if (bossCam == null)
        {
            return;
        }

        // Find SceneLimiter child object
        Transform limiterTransform = transform.Find("BossSceneLimiter");
        if (limiterTransform == null)
        {
            return;
        }

        // Get BoxCollider2D from SceneLimiter
        sceneLimiterCollider = limiterTransform.GetComponent<BoxCollider2D>();
        if (sceneLimiterCollider == null)
        {
            return;
        }

        if (bossHPBar != null) bossHPBar.SetActive(false);
        sceneLimiterCollider.isTrigger = true; // Start as trigger
        bossCam.Priority = 0;  // Ensure boss cam is inactive initially
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            bossCam.Priority = 20; // Switch to boss cam
            sceneLimiterCollider.isTrigger = false; // Make it solid barrier
            StartCoroutine(ShowHPBarAfterDelay());
        }
    }

    private IEnumerator ShowHPBarAfterDelay()
    {
        yield return new WaitForSeconds(delayBeforeHPBar);

        if (bossHPBar != null)
            bossHPBar.SetActive(true); // Show HP bar after delay
    }
}
