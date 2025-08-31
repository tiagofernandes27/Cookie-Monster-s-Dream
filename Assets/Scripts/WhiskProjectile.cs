using UnityEngine;

public class WhiskProjectile : Projectile
{


    private Vector2 launchDirection;
    private float launchSpeed;


    private void FixedUpdate()
    {
        if (projectileRigidbody2D != null)
        {
            projectileRigidbody2D.linearVelocity = launchDirection * launchSpeed;
        }
    }

    public override void Launch(Vector2 direction, float speed, float damageAmount)
    {
        launchDirection = direction.normalized;
        launchSpeed = speed;
        damage = damageAmount;

        FlipVisual(direction);
    }

    protected override void OnHit(Collider2D collision)
    {
        //Debug.Log($"Straight projectile hit {collision.name}");
        // TODO: Apply weapon damage here
        collision.GetComponent<Enemy>().TakeDamage(damage);
        Destroy(gameObject);
    }


}
