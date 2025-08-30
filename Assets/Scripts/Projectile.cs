using UnityEngine;

public abstract class Projectile : MonoBehaviour
{


    [SerializeField] protected float lifeTime = 5f;
    [SerializeField] protected LayerMask hitLayers;


    protected Rigidbody2D projectileRigidbody2D;


    protected virtual void Awake()
    {
        projectileRigidbody2D = GetComponent<Rigidbody2D>();
    }

    protected virtual void Start()
    {
        Destroy(gameObject, lifeTime);
    }

    protected virtual void OnTriggerEnter2D(Collider2D collision)
    {
        if ((hitLayers.value & (1 << collision.gameObject.layer)) != 0)
        {
            OnHit(collision);
        }
    }

    // Each subclass defines what happens when it hits something
    protected abstract void OnHit(Collider2D collision);

    // Launch method to be called by the weapon
    public abstract void Launch(Vector2 direction, float speed);


}
