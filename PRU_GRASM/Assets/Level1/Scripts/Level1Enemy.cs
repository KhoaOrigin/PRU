using UnityEngine;
using UnityEngine.UI;
public abstract class Level1Enemy : MonoBehaviour
{
    [SerializeField] protected float enemyMoveSpeed = 1f;
    protected Level1Player Level1Player;
    [SerializeField] protected float maxHp = 50f;
    protected float currentHp;
    [SerializeField] private Image hpBar;
    [SerializeField] protected float enterDamage = 10f;
    [SerializeField] protected float stayDamage = 1f;

    protected virtual void Start()
    {
        Level1Player = FindObjectOfType<Level1Player>();
        currentHp = maxHp;
        UpdateHpBar();
    }

    protected virtual void Update()
    {
        MoveToPlayer();
    }

    protected void MoveToPlayer()
    {
        if (Level1Player != null)
        {
            transform.position = Vector2.MoveTowards(transform.position, Level1Player.transform.position, enemyMoveSpeed*Time.deltaTime);
            FlipEnemy();
        }
    }

    protected void FlipEnemy()
    {
        if (Level1Player != null)
        {
            transform.localScale = new Vector3(Level1Player.transform.localScale.x < transform.position.x ? -1 : 1,1,1);
        }
    }

    public virtual void TakeDamage(float damage)
    {
        currentHp -= damage;
        currentHp = Mathf.Max(currentHp, 0);
        UpdateHpBar();
        if (currentHp <= 0)
        {
            Die();
        }
    }

    protected virtual void Die()
    {
        Destroy(gameObject);
    }

    protected void UpdateHpBar()
    {
        if (hpBar != null)
        {
            hpBar.fillAmount = currentHp / maxHp;
        }
    }
}
