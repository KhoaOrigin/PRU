using UnityEngine;
using UnityEngine.UI;

public abstract class Enemy : MonoBehaviour
{
    [SerializeField] private Image hpBar;
    [SerializeField] protected float enemyMoveSpeed = 1f;
    [SerializeField] protected float maxHp = 50f;
    [SerializeField] protected float entryDamage = 10f;
    [SerializeField] protected float overtimeDamage = 5f;
    protected float currentHp;
    protected PlayerScript player;

    protected virtual void Start()
    {
        player = FindAnyObjectByType<PlayerScript>();
        currentHp = maxHp;
        RenderHpBar();
    }

    protected virtual void Update()
    {
        MoveToPlayer();
    }

    protected virtual void MoveToPlayer()
    {
        if (player == null)
        {
            return;
        }

        transform.position = Vector2.MoveTowards(transform.position, player.transform.position, enemyMoveSpeed * Time.deltaTime);
        Flip();
    }

    protected void Flip()
    {
        if (player == null)
        {
            return;
        }

        transform.localScale = new Vector3(player.transform.position.x < transform.position.x ? -1 : 1, 1, 1);
    }

    public virtual void TakeDamage(float damage)
    {
        currentHp -= damage;
        currentHp = Mathf.Max(currentHp, 0);

        RenderHpBar();

        if (currentHp <= 0)
        {
            Die();
        }
    }

    protected virtual void Die()
    {
        Destroy(gameObject);
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
