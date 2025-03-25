using UnityEngine;
using Unity.Cinemachine;
using System.Collections;

public class BossCameraTrigger : MonoBehaviour
{
    public CinemachineCamera bossCam;  // Assign in Inspector
    private BoxCollider2D sceneLimiterCollider;
    public GameObject bossHPBar; // Assign HP Bar (Canvas) in Inspector
    public float delayBeforeHPBar = 2.0f; // Adjust delay as needed
    public GameObject boss;
    public GameObject enemySpawner;

    [SerializeField] private AudioClip bossTheme;

    private AudioSource mapAudioSource;
    private AudioSource bossAudioSource;

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

        if (boss != null) boss.SetActive(false);
        if (enemySpawner != null) enemySpawner.SetActive(false);

        // Get Map's AudioSource
        GameObject map = GameObject.Find("Map");
        if (map != null)
        {
            mapAudioSource = map.GetComponent<AudioSource>();
        }

        // Get or add AudioSource for boss theme
        bossAudioSource = GetComponent<AudioSource>();
        if (bossAudioSource == null)
        {
            bossAudioSource = gameObject.AddComponent<AudioSource>();
        }
        bossAudioSource.clip = bossTheme;
        bossAudioSource.loop = true; // Loop the boss theme
        bossAudioSource.playOnAwake = false;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            boss.SetActive(true);
            enemySpawner.SetActive(true);
            mapAudioSource.Stop();
            bossAudioSource.Play();
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
