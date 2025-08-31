using UnityEngine;

public class EnemyProjectile : MonoBehaviour
{


    [SerializeField] private float speed = 5f;
    [SerializeField] private float lifetime = 3f;

    [SerializeField] private LayerMask hitLayers;


    private int damage;
    private Vector2 direction;
    private Rigidbody2D projectileRigidbody2D;


    private void Awake()
    {
        projectileRigidbody2D = GetComponent<Rigidbody2D>();
    }

    public void Shoot(Vector2 shootDirection, int damageAmount)
    {
        direction = shootDirection.normalized;
        damage = damageAmount;
        Destroy(gameObject, lifetime);
    }

    private void FixedUpdate()
    {
        if (projectileRigidbody2D != null)
        {
            projectileRigidbody2D.linearVelocity = direction * speed;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if ((hitLayers.value & (1 << collision.gameObject.layer)) != 0)
        {
            var playerHealth = collision.GetComponent<PlayerHealth>();
            if (playerHealth != null)
                playerHealth.ChangeHealth(-damage);

            Destroy(gameObject);
        }
    }


}
