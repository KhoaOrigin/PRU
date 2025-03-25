using System.Collections;
using UnityEngine;
using UnityEngine.Splines;
using UnityEngine.UI;

public class PlayerScript : MonoBehaviour
{   
    [SerializeField] private Image hpBar;
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] protected float maxHp = 100f;
    protected float currentHp;

    private Rigidbody2D rb;
    private SpriteRenderer rbSprite;
    private Animator animator;

    [SerializeField] private float invincibilityTime = 1f; // Time of invincibility
    [SerializeField] private float blinkInterval = 0.1f;  // How fast the player blinks
    [SerializeField] private float knockbackDuration = 0.2f; // Knockback effect time
    [SerializeField] private GameObject gameOverCanvas;

    private bool isInvincible = false;
    private bool isKnockedBack = false;



    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        rbSprite = rb.GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();

        currentHp = maxHp;
        RenderHpBar();
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (!isKnockedBack) // Prevent movement while in knockback
        {
            Move();
        }
    }

    void Move()
    {
        Vector2 playerInput = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));

        rb.linearVelocity = playerInput.normalized * moveSpeed;


        if (playerInput.x < 0)
        {
            rbSprite.flipX = true;
        } else if (playerInput.x > 0)
        {
            rbSprite.flipX = false;
        }

        if (playerInput != Vector2.zero)
        {
            animator.SetFloat("Move", 1);
        } else if (playerInput == Vector2.zero)
        {
            animator.SetFloat("Move", 0);
        }
  
    }

    public void TakeDamage(float damage)
    {
        currentHp -= damage;
        currentHp = Mathf.Max(currentHp, 0);

        RenderHpBar();

        if (currentHp <= 0)
        {
            Die();
        }
    }

    public void TakeDamage(float damage, Vector2 knockbackForce)
    {
        if (isInvincible) return; // Ignore damage if already in I-frames

        currentHp -= damage;
        currentHp = Mathf.Max(currentHp, 0);
        RenderHpBar();

        StartCoroutine(ApplyKnockback(knockbackForce));
        StartCoroutine(InvincibilityFrames());

        if (currentHp <= 0)
            Die();
    }

    private IEnumerator ApplyKnockback(Vector2 force)
    {
        isKnockedBack = true;
        rb.linearVelocity = Vector2.zero; // Stop current movement
        rb.AddForce(force, ForceMode2D.Impulse); // Apply knockback force

        yield return new WaitForSeconds(knockbackDuration); // Knockback lasts for a short time

        rb.linearVelocity = Vector2.zero; // Reset velocity after knockback
        isKnockedBack = false;
    }

    private IEnumerator InvincibilityFrames()
    {
        isInvincible = true;

        float elapsedTime = 0f;
        while (elapsedTime < invincibilityTime)
        {
            rbSprite.enabled = !rbSprite.enabled; // Blink effect
            yield return new WaitForSeconds(blinkInterval);
            elapsedTime += blinkInterval;
        }

        rbSprite.enabled = true;
        isInvincible = false;
    }

    public void Die()
    {
        if (gameOverCanvas != null)
        {
            gameOverCanvas.SetActive(true);
            Time.timeScale = 0f; // Pause the game
            Debug.Log("Game Over menu displayed");
        }
        //Destroy(gameObject);
    }

    public void RestartGame()
    {
        Time.timeScale = 1f; // Resume time
        UnityEngine.SceneManagement.SceneManager.LoadScene(
            UnityEngine.SceneManagement.SceneManager.GetActiveScene().name
        );
    }

    protected void RenderHpBar()
    {
        if (hpBar == null)
        {
            return;
        }

        hpBar.fillAmount = currentHp / maxHp;
    }

}
