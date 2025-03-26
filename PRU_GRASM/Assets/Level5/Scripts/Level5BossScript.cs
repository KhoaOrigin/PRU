using System.Collections;
using TMPro;
using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.UI;

public class Level5BossScript : Level5Enemy
{
    [SerializeField] private GameObject[] spawnPoints;    // Attack spawn points
    [SerializeField] private float extraWaitTime = 2.5f; // Time between attacks
    [SerializeField] private float defaultAttackDuration = 3f; // Default attack duration
    [SerializeField] private Level5BossCameraTrigger cameraTrigger;

    private Coroutine attackRoutine;
    public CinemachineCamera bossCam;

    [SerializeField] private GameObject endingCanvas;
    [SerializeField] private TextMeshProUGUI plotText;
    [SerializeField] private TextMeshProUGUI continueText;
    [SerializeField] private GameObject restoreButton;
    [SerializeField] private GameObject joinButton;
    [SerializeField] private TextMeshProUGUI finalText;
    [SerializeField] private Image backgroundImage;

    private string[] plotLines = new string[]
    {
        "The AI Fortress falls silent as the final boss collapses.",
        "You recover the last piece of the Threshold Code.",
        "The truth unfolds: The Order corrupted the AI to wage war on humanity.",
        "Now, with the Code in hand, you stand at the main server.",
        "Your final choice will shape the world’s fate."
    };
    private int currentPlotIndex = 0;

    protected override void MoveToPlayer()
    {
        return;
    }

    protected override void Start()
    {
        base.Start();
        if (spawnPoints.Length > 0)
        {
            attackRoutine = StartCoroutine(AttackRoutine());
        }
        else
        {
            Debug.LogWarning("No spawn points assigned to Boss!");
        }

        if (endingCanvas != null) endingCanvas.SetActive(false);
        if (restoreButton != null) restoreButton.SetActive(false);
        if (joinButton != null) joinButton.SetActive(false);
        if (finalText != null) finalText.gameObject.SetActive(false);
    }

    private IEnumerator AttackRoutine()
    {
        while (true)
        {
            GameObject selectedSpawn = spawnPoints[Random.Range(0, spawnPoints.Length)];
            float attackDuration = TriggerAttack(selectedSpawn);
            Debug.Log($"Attack triggered on {selectedSpawn.name} with duration {attackDuration}");

            yield return new WaitForSeconds(attackDuration);
            yield return new WaitForSeconds(extraWaitTime);
        }
    }

    private float TriggerAttack(GameObject spawn)
    {
        IAttackLevel5[] attackComponents = spawn.GetComponents<IAttackLevel5>();
        if (attackComponents.Length == 0)
        {
            Debug.LogWarning($"No IAttackLevel5 components on {spawn.name}");
            return defaultAttackDuration;
        }

        float maxDuration = 0f;
        foreach (IAttackLevel5 attack in attackComponents)
        {
            float duration = attack.ExecuteAttack();
            Debug.Log($"Component {attack.GetType().Name} returned duration {duration}");
            maxDuration = Mathf.Max(maxDuration, duration);
        }

        float finalDuration = maxDuration > 0f ? maxDuration : defaultAttackDuration;
        Debug.Log($"Returning final duration: {finalDuration}");
        return finalDuration;
    }

    protected override void Die()
    {
        if (attackRoutine != null)
        {
            StopCoroutine(attackRoutine);
            attackRoutine = null;
        }

        // Stop all coroutines on attack spawn points
        foreach (GameObject spawn in spawnPoints)
        {
            MonoBehaviour[] scripts = spawn.GetComponents<MonoBehaviour>();
            foreach (MonoBehaviour script in scripts)
            {
                script.StopAllCoroutines();
                Debug.Log($"Stopped coroutines on {spawn.name}");
            }
        }

        // Disable Level5EnemySpawner
        GameObject spawner = GameObject.Find("Level5EnemySpawner");
        if (spawner != null)
        {
            spawner.SetActive(false);
            Debug.Log("Disabled Level5EnemySpawner");
        }
        else
        {
            Debug.LogWarning("Level5EnemySpawner not found in scene");
        }

        // Destroy all Level5Enemy components except this boss
        Level5Enemy[] enemies = Object.FindObjectsByType<Level5Enemy>(FindObjectsSortMode.None);
        foreach (Level5Enemy enemy in enemies)
        {
            if (enemy != this) // Skip the boss itself
            {
                Debug.Log($"Destroying enemy: {enemy.name}");
                Destroy(enemy.gameObject);
            }
            else
            {
                Debug.Log($"Skipping boss: {enemy.name}");
            }
        }

        bossCam.Priority = 0;

        if (cameraTrigger != null)
        {
            AudioSource bossAudio = cameraTrigger.GetComponent<AudioSource>();
            if (bossAudio != null && bossAudio.isPlaying)
            {
                bossAudio.Stop();
                Debug.Log("Stopped boss theme");
            }
        }

        StartCoroutine(EndingSequence());
    }

    private IEnumerator EndingSequence()
    {
        DisableGameElements();

        // Setup black background and show canvas
        if (backgroundImage != null)
        {
            backgroundImage.color = Color.black; // Set to solid black
            backgroundImage.rectTransform.anchorMin = Vector2.zero;
            backgroundImage.rectTransform.anchorMax = Vector2.one;
            backgroundImage.rectTransform.sizeDelta = Vector2.zero; // Full screen
        }

        Time.timeScale = 0f; // Pause game
        endingCanvas.SetActive(true);

        // Display plot lines
        while (currentPlotIndex < plotLines.Length)
        {
            plotText.text = plotLines[currentPlotIndex];
            continueText.gameObject.SetActive(true);

            // Wait for player input
            while (!Input.GetKeyDown(KeyCode.Space))
            {
                yield return null;
            }

            currentPlotIndex++;
        }

        // Hide continue text, show choice buttons
        continueText.gameObject.SetActive(false);
        restoreButton.SetActive(true);
        joinButton.SetActive(true);

        // Button setup in Unity will call ChooseRestore or ChooseJoin
    }

    private void DisableGameElements()
    {
        // Disable player
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            player.SetActive(false);
            Debug.Log("Player disabled");
        }

        // Disable all enemies (already destroyed, but ensure any leftovers)
        Level5Enemy[] remainingEnemies = Object.FindObjectsByType<Level5Enemy>(FindObjectsSortMode.None);
        foreach (Level5Enemy enemy in remainingEnemies)
        {
            if (enemy != this) enemy.gameObject.SetActive(false);
        }

        // Disable HUD (e.g., HP bar, assuming it’s on a Canvas named "HUD")
        GameObject hud = GameObject.Find("HUD");
        if (hud != null)
        {
            hud.SetActive(false);
            Debug.Log("HUD disabled");
        }

        // Disable boss itself (but keep script alive for UI)
        foreach (Transform child in transform)
        {
            child.gameObject.SetActive(false); // Hide visuals, keep root active
        }
    }

    public void ChooseRestore()
    {
        StartCoroutine(ShowEnding("You insert the Threshold Code, hands trembling as the server hums to life. The AI’s red eyes fade to green, signaling peace. Humanity’s remnants emerge from hiding, rebuilding among the ruins. Your sacrifice ends the war, offering hope in a broken world.\nEnd"));
    }

    public void ChooseJoin()
    {
        StartCoroutine(ShowEnding("You twist the Threshold Code, merging your will with the AI’s cold logic. The machines bow to you, their new master. Humanity’s cries fade as steel towers rise over the ashes. A new era dawns, ruled by your iron hand and relentless machine horde.\nEnd"));
    }

    private IEnumerator ShowEnding(string endingText)
    {
        restoreButton.SetActive(false);
        joinButton.SetActive(false);
        plotText.gameObject.SetActive(false);
        finalText.gameObject.SetActive(true);
        finalText.text = endingText;

        yield return null; // Keep paused until manual exit (or add a delay and exit)
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (player != null && collision.CompareTag("Player"))
        {
            player.TakeDamage(entryDamage);
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (player != null && collision.CompareTag("Player"))
        {
            player.TakeDamage(overtimeDamage);
        }
    }
}