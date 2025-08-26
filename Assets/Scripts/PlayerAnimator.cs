using UnityEngine;

public class PlayerAnimator : MonoBehaviour
{


    private const string IS_RUNNING = "IsRunning";


    private Animator animator;
    private SpriteRenderer spriteRenderer;


    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
        animator.SetBool(IS_RUNNING, Player.Instance.IsRunning());
    }

    private void Update()
    {
        animator.SetBool(IS_RUNNING, Player.Instance.IsRunning());

        // Flip sprite based on facing direction
        spriteRenderer.flipX = Player.Instance.IsFacingRight();
    }


}
