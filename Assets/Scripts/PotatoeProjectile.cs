using UnityEngine;

public class PotatoeProjectile : Projectile
{


    private const string IS_EXPLODING = "IsExploding";


    [SerializeField] private float explosionRadius = 3f;
    [SerializeField] private Animator animator;
    [SerializeField] private LayerMask enemyLayer;


    private Vector2 launchDirection;
    private float launchSpeed;
    private bool hasExploded = false;


    private void FixedUpdate()
    {
        if (!hasExploded && projectileRigidbody2D != null)
        {
            projectileRigidbody2D.linearVelocity = launchDirection * launchSpeed;
        }
    }

    public override void Launch(Vector2 direction, float speed, float damageAmount)
    {
        launchDirection = direction.normalized;
        launchSpeed = speed;
        damage = damageAmount;

        hasExploded = false;

        FlipVisual(direction);
    }

    protected override void OnHit(Collider2D collision)
    {
        if (hasExploded) return; // prevent multiple triggers
        hasExploded = true;

        //Debug.Log($"Potato hit on {collision.name}");

        if (projectileRigidbody2D != null)
            projectileRigidbody2D.linearVelocity = Vector2.zero;

        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, explosionRadius, enemyLayer);
        foreach (Collider2D hit in hits)
        {
            //Debug.Log($"Explosion hit: {hit.name}");
            // TODO: Apply damage from Weapon
            hit.GetComponent<Enemy>().TakeDamage(damage);
        }

        if (animator != null)
        {
            animator.SetTrigger(IS_EXPLODING);
        }
        else
        {
            Destroy(gameObject); // fallback: destroy immediately
        }
    }

    // Called by Animation Event at the end of the explosion anim
    public void DestroyAfterExplosion()
    {
        Destroy(gameObject);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, explosionRadius);
    }


}
