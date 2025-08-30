using UnityEngine;

public class SpitProjectile : Projectile
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

    public override void Launch(Vector2 direction, float speed)
    {
        launchDirection = direction.normalized;
        launchSpeed = speed;
    }

    protected override void OnHit(Collider2D collision)
    {
        Debug.Log($"Straight projectile hit {collision.name}");
        // TODO: Apply weapon damage here
        Destroy(gameObject);
    }


}
