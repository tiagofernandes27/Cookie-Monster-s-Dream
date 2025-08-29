using UnityEngine;

public class SpitingWeapon : Weapon
{


    [SerializeField] private GameObject projectilePrefab;
    [SerializeField] private float projectileSpeed = 12f;

    protected override void PerformAttack()
    {
        Debug.Log("Shoot with Spiting!");

        // Spawn projectile
        GameObject proj = Instantiate(projectilePrefab, transform.position, transform.rotation);

        Rigidbody2D rb = proj.GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            rb.linearVelocity = transform.right * projectileSpeed;
        }
    }


}
