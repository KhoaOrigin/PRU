using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public abstract class Enemy : MonoBehaviour
{
    [SerializeField] protected float enemySpeed=1f;
    protected Player player;
    [SerializeField] protected float maxHP = 40f;
    protected float currentHP;
    [SerializeField] protected Image hpBar;
    [SerializeField] protected float Damage = 10f;
    [SerializeField] protected Coroutine damageCoroutine;
    private bool isTouchingPlayer = false;
    private bool canMove = true;
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            isTouchingPlayer = true;
            StartCoroutine(DamageOverTime());
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            isTouchingPlayer = false;
        }
    }

    private IEnumerator DamageOverTime()
    {
        while (isTouchingPlayer)
        {
            if (player != null && canMove)
            {
                player.TakeDame(Damage);
                StartCoroutine(StopEnemyForSeconds(2f)); // Dừng kẻ địch 2 giây
            }
            yield return new WaitForSeconds(2f); // Lặp lại mỗi 2 giây
        }
    }

    private IEnumerator StopEnemyForSeconds(float duration)
    {
        canMove = false;
        damageCoroutine = null;
        yield return new WaitForSeconds(duration);
        canMove = true;
    }
    protected virtual void Start()
    {
        player = FindAnyObjectByType<Player>();
        currentHP = maxHP;
        updateHpBar();
    }
    protected virtual void Update()
    {
        MoveToPlayer();
    }
    protected void MoveToPlayer()
    {
        if (player != null)
        {
            transform.position = Vector2.MoveTowards(transform.position, player.transform.position, enemySpeed * Time.deltaTime);
            FlipEnemy();
        }
    }
    protected void FlipEnemy()
    {
        if (player != null)
        {
            transform.localScale = new Vector3(player.transform.position.x < transform.position.x?-1 :1,1,1);
        }
    }
    public virtual void TakeDamage(float damage)
    {
        currentHP -= damage;
        currentHP = Mathf.Max(currentHP, 0);
        updateHpBar();
        if (currentHP <= 0)
        {
            Die();
        }
    }
    protected virtual void Die()
    {
        Destroy(gameObject);
    }
    protected void updateHpBar()
    {
        if (hpBar != null)
        {
            hpBar.fillAmount = currentHP / maxHP;
        }
    }
}
