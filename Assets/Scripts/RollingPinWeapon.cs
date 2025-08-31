using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class RollingPinWeapon : Weapon
{


    [Header("Attack Settings")]
    [SerializeField] private Collider2D hitbox;
    [SerializeField] private LayerMask enemyLayer;
    [SerializeField] private int maxHitsPerEnemy = 1; // Set in Inspector


    // Keep track of how many times each enemy has been hit during this swing
    private Dictionary<Collider2D, int> enemyHitCounts = new Dictionary<Collider2D, int>();


    protected override void Start()
    {
        base.Start();

        DisableHitbox();
    }

    protected override void PerformAttack()
    {
        // Just play the attack animation
        //Debug.Log("Rolling Pin Attack!");
        // Animator trigger is already handled in base.Attack()

        // Reset hit counts for this new swing
        enemyHitCounts.Clear();
    }

    // Animation events call these
    public void EnableHitbox()
    {
        if (hitbox != null) hitbox.enabled = true;
    }

    public void DisableHitbox()
    {
        if (hitbox != null) hitbox.enabled = false;
    }

    public void OnHitCollider(Collider2D other)
    {
        if (!(((1 << other.gameObject.layer) & enemyLayer) != 0))
            return;

        // Track hit counts
        if (!enemyHitCounts.ContainsKey(other))
            enemyHitCounts[other] = 0;

        if (enemyHitCounts[other] >= maxHitsPerEnemy)
            return;

        enemyHitCounts[other]++;
        //Debug.Log($"Rolling Pin hit {other.name} ({enemyHitCounts[other]}/{maxHitsPerEnemy})");

        // TODO: Apply damage
        other.GetComponent<Enemy>().TakeDamage(damage);
    }


}
