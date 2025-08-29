using UnityEngine;

public class PlayerAnimator : MonoBehaviour
{


    private const string IS_RUNNING = "IsRunning";


    private Animator animator;
    private SpriteRenderer spriteRenderer;
    private TrailRenderer trailRenderer;
    private bool trailRendererActive = false;


    private void Awake()
    {
        trailRenderer = GetComponent<TrailRenderer>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
        animator.SetBool(IS_RUNNING, Player.Instance.IsRunning());
    }

    private void Update()
    {
        animator.SetBool(IS_RUNNING, Player.Instance.IsRunning());

        // Flip sprite based on facing direction
        spriteRenderer.flipX = !Player.Instance.IsFacingRight();

        // Enable/disable trail only when state changes
        bool shouldEmit = Player.Instance.IsDashing();
        if (trailRendererActive != shouldEmit)
        {
            trailRenderer.emitting = shouldEmit;

            trailRendererActive = shouldEmit;
        }
    }


}
