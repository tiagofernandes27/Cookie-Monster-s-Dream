using UnityEngine;

public class RotatingWhiskWeapon : RangedWeapon
{


    [Header("Whisk Settings")]
    [SerializeField] private float projectileRandomSpawnY = 0.5f;


    protected override void SpawnProjectile()
    {
        if (projectilePrefab == null) return;

        // Apply random offset in Y
        Vector3 spawnPos = transform.position;
        spawnPos.y += Random.Range(-projectileRandomSpawnY, projectileRandomSpawnY);

        GameObject projectile = Instantiate(projectilePrefab, spawnPos, transform.rotation);

        Projectile projectileComponent = projectile.GetComponent<Projectile>();
        if (projectileComponent != null)
        {
            projectileComponent.Launch(transform.right, projectileSpeed, damage);
        }
    }


}
