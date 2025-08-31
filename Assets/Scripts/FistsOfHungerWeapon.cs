using UnityEngine;
using UnityEngine.InputSystem;

public class FistsOfHungerWeapon : Weapon
{


    [Header("Attack Settings")]
    [SerializeField] private float attackRange = 1.2f;
    [SerializeField] private float attackRadius = 0.5f;
    [SerializeField] private float attackOffset = 0.2f;
    [SerializeField] private LayerMask enemyLayer;


    protected override void PerformAttack()
    {
        Vector2 playerPos = Player.Instance.transform.position;
        Vector2 weaponPos = transform.position;
        Vector2 dir = (weaponPos - playerPos).normalized; // from player to weapon

        // --- Calculate attack center ---
        // attackRange = full forward reach of the hitbox
        // attackRange / 2f = halfway point (center of the box)
        // Example: if attackRange = 4 ? box extends 4 units forward ? center is 2 units from player
        // attackOffset = optional extra push forward to avoid overlap with player
        Vector2 attackCenter = playerPos + dir * (attackRange /2f + attackOffset);

        // --- Define box size ---
        // attackRange = full length forward
        // attackRadius = half of the vertical "thickness" of the hitbox
        // attackRadius * 2f = full vertical size Unity requires
        // Example: if attackRadius = 0.5 ? box extends 0.5 up & 0.5 down ? total height = 1
        Vector2 boxSize = new Vector2(attackRange, attackRadius * 2f);

        // Rotate box along the facing direction
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;

        Collider2D[] hits = Physics2D.OverlapBoxAll(attackCenter, boxSize, angle, enemyLayer);
        foreach (Collider2D hit in hits)
        {
            // TODO: enemy damage logic
            hit.GetComponent<Enemy>()?.TakeDamage(damage);
        }
    }

    private void OnDrawGizmosSelected()
    {
        if (Player.Instance == null) return;

        Vector2 playerPos = Player.Instance.transform.position;
        Vector2 weaponPos = transform.position;

        Vector2 dir = (weaponPos - playerPos).normalized;

        // Center of capsule for gizmos matches attack
        Vector2 attackCenter = playerPos + dir * (attackRange * 0.5f + attackOffset);

        Gizmos.color = Color.red;
        Matrix4x4 oldMatrix = Gizmos.matrix;
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        Gizmos.matrix = Matrix4x4.TRS(attackCenter, Quaternion.Euler(0, 0, angle), Vector3.one);

        // Draw rectangle only (matches capsule)
        Gizmos.DrawWireCube(Vector3.zero, new Vector3(attackRange, attackRadius * 2f, 0));

        Gizmos.matrix = oldMatrix;
    }


}
