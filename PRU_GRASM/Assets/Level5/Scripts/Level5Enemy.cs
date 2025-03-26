using UnityEngine;
using UnityEngine.UI;

public abstract class Level5Enemy : MonoBehaviour
{
    [SerializeField] private Image hpBar;
    [SerializeField] protected float enemyMoveSpeed = 1f;
    [SerializeField] protected float maxHp = 50f;
    [SerializeField] protected float entryDamage = 10f;
    [SerializeField] protected float overtimeDamage = 5f;
    protected float currentHp;
    protected Level5PlayerScript player;

    protected virtual void Start()
    {
        player = FindAnyObjectByType<Level5PlayerScript>();
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
        Debug.Log($"{name} is dying");
        OnDie(); // Notify components before destruction
        Destroy(gameObject);
    }

    protected virtual void OnDie()
    {
        Level5EnemyWeaponScript weapon = GetComponentInChildren<Level5EnemyWeaponScript>();
        if (weapon != null)
        {
            Debug.Log($"Found Level5EnemyWeaponScript on {name}, calling OnEnemyDeath");
            weapon.OnEnemyDeath();
        }
        else
        {
            Debug.LogWarning($"No Level5EnemyWeaponScript found on {name}");
        }
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
