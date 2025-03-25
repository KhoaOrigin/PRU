using UnityEngine;

public class BossEliteGunnerMinionScript : Enemy
{

    protected override void MoveToPlayer()
    {
        return;
    }

    protected override void Die()
    {
        Debug.Log($"{name} (BossEliteGunnerMinionScript) overriding Die");
        OnDie(); // Ensure cleanup happens
        base.Die(); // Call base to destroy the GameObject
    }

    protected override void OnDie()
    {
        Debug.Log($"{name} (BossEliteGunnerMinionScript) calling OnDie");
        base.OnDie(); // Call base to handle weapon cleanup
    }

}
