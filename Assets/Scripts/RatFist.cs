using TMPro;
using UnityEngine;

public class RatFist : MonoBehaviour
{


    [SerializeField] private float attackRange = 1.5f;
    [SerializeField] private LayerMask hitLayers; // Usually Player layer


    private Animator animator;

    public float AttackRange => attackRange;


    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    public void Swing(int damage, Vector2 targetPosition)
    {
        Vector2 direction = (targetPosition - (Vector2)transform.position).normalized;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0f, 0f, angle);

        Vector3 originalPosition = transform.position;

        transform.position += (Vector3)(direction * 0.5f);

        // Detect player in range
        Collider2D hit = Physics2D.OverlapCircle(transform.position, attackRange, hitLayers);
        if (hit != null)
        {
            hit.GetComponent<PlayerHealth>()?.ChangeHealth(-damage);
            Debug.Log("Hit" + hit);
        }

        animator.SetTrigger("Attack");
        // Optional: play sound, VFX, etc.

        transform.position = originalPosition;
    }

    // For visualizing the attack range in editor
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }


}
