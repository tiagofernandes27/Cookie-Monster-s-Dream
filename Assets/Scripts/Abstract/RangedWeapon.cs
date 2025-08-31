using UnityEngine;
using UnityEngine.InputSystem;

public abstract class RangedWeapon : Weapon
{


    [Header("Projectile Settings")]
    [SerializeField] protected GameObject projectilePrefab;
    [SerializeField] protected float projectileSpeed = 12f;


    protected override void PerformAttack()
    {
        if (projectilePrefab == null)
        {
            Debug.LogWarning("Projectile prefab not assigned!");
            return;
        }

        SpawnProjectile();
    }

    // Each ranged weapon can override this if it wants custom behavior
    protected virtual void SpawnProjectile()
    {
        GameObject projectile = Instantiate(projectilePrefab, transform.position, transform.rotation);

        Projectile projectileComponent = projectile.GetComponent<Projectile>();
        if (projectileComponent != null)
        {
            projectileComponent.Launch(transform.right, projectileSpeed, damage);
        }
    }


}
