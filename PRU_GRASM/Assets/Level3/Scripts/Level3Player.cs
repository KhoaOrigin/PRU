using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 5f;
    private Rigidbody2D rb;
    private SpriteRenderer SpriteRenderer;
    private Animator animator;
    [SerializeField] private Image hpBar;
    [SerializeField] private float maxHP = 100f;
    private float currentHP;
    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        SpriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
    }
    void Start()
    {
        currentHP = maxHP;
        updateHpBar();
    }

    void Update()
    {
        MovePlayer();
    }
    void MovePlayer()
    {
        Vector2 playerInput = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        rb.linearVelocity = playerInput.normalized * moveSpeed;
        if (playerInput.x > 0)
        {
            SpriteRenderer.flipX = false;
        }
        else if (playerInput.x < 0)
        {
            SpriteRenderer.flipX = true;
        }
        if(playerInput != Vector2.zero)
        {
            animator.SetBool("IsRun", true);
        }
        else
        {
            animator.SetBool("IsRun", false);
        }
    }
    public void TakeDame(float damege)
    {
        currentHP -= damege;
        currentHP = Mathf.Max(currentHP, 0);
        updateHpBar();
        if (currentHP <= 0)
        {
            Die();
        }
    }
    public void heal(float healValue)
    {
        if(currentHP< maxHP)
        {
            currentHP += healValue;
            currentHP = Mathf.Min(currentHP, maxHP);
            updateHpBar();
        }
    }
    private void Die()
    {
        Destroy(gameObject);
    }
    private void updateHpBar()
    {
        if (hpBar != null)
        {
            hpBar.fillAmount = currentHP / maxHP;
        }
    }
}
