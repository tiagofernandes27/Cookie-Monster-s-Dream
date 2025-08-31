using UnityEngine;

public class EnemyRatMovement : MonoBehaviour
{


    [SerializeField] private float speed;
    [SerializeField] private SpriteRenderer spriteRenderer;


    private bool isWalking;
    private EnemyState enemyState;
    private Rigidbody2D enemyRigidbody2D;
    private Transform player;


    private void Start()
    {
        enemyRigidbody2D = GetComponent<Rigidbody2D>();
        player = Player.Instance.transform;

        ChangeState(EnemyState.Idle);
    }

    private void Update()
    {
        Vector2 direction = (player.position - transform.position).normalized;
        enemyRigidbody2D.linearVelocity = direction * speed;

        // Flip sprite depending on movement
        if (spriteRenderer != null)
        {
            if (direction.x > 0.01f)
                spriteRenderer.flipX = false; // facing right
            else if (direction.x < -0.01f)
                spriteRenderer.flipX = true;  // facing left
        }
    }

    private void ChangeState(EnemyState newState)
    {
        enemyState = newState;

        if (enemyState == EnemyState.Idle)
            isWalking = false;

        if (enemyState == EnemyState.Chasing)
            isWalking = true;
    }

    public bool IsWalking()
    {
        return isWalking;
    }


}

public enum EnemyState
{
    Idle,
    Chasing,
}
