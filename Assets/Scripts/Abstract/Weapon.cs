using UnityEngine;

public abstract class Weapon : MonoBehaviour
{


    private const string ATTACK = "Attack";
    private const string ATTACK_SPEED = "AttackSpeed";


    [Header("Position")]
    [SerializeField] private float radius = 1.5f;

    [Header("Stats")]
    [SerializeField] protected float damage = 10f;
    [SerializeField] protected float attackSpeed = 1f; // attacks per second

    [Header("References")]
    [SerializeField] private Animator animator;
    [SerializeField] private SpriteRenderer visualSpriteRenderer;

    public enum FlipMode { None, FlipX, FlipY, Both }

    [Header("Visual Settings")]
    [SerializeField] private FlipMode flipMode = FlipMode.FlipY;


    // Controlled by WeaponHolder
    private bool isActive = false;


    protected float attackCooldown = 0f;
    protected Transform playerTransform;
    protected float baseAttackSpeedCooldown = 1.5f;


    protected virtual void Start()
    {
        playerTransform = Player.Instance.transform; // cache reference
    }

    protected virtual void Update()
    {
        RotateAroundPlayer();

        if (attackCooldown > 0)
        {
            attackCooldown -= Time.deltaTime;
        }
    }

    private void RotateAroundPlayer()
    {
        if (Player.Instance == null) return;

        Vector3 playerPos = Player.Instance.transform.position;

        // Get direction from player to mouse
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePos.z = 0;
        Vector3 dir = (mousePos - playerPos).normalized;

        // Calculate position around the player at fixed radius
        transform.position = playerPos + dir * radius;

        // Calculate signed angle for rotation
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, angle);

        // Flip visual based on mouse side
        switch (flipMode)
        {
            case FlipMode.FlipY:
                visualSpriteRenderer.flipY = dir.x < 0;
                visualSpriteRenderer.flipX = false;
                break;
            case FlipMode.FlipX:
                visualSpriteRenderer.flipX = dir.x < 0;
                visualSpriteRenderer.flipY = false;
                break;
            case FlipMode.Both:
                visualSpriteRenderer.flipX = dir.x < 0;
                visualSpriteRenderer.flipY = dir.x < 0;
                break;
            case FlipMode.None:
                visualSpriteRenderer.flipX = false;
                visualSpriteRenderer.flipY = false;
                break;
        }
    }

    private bool CanAttack()
    {
        return attackCooldown <= 0f;
    }


    // Public method called by input
    public void Attack()
    {
        // Only allow attacking if active
        if (!isActive) return;

        if (!CanAttack()) return;

        //Debug.Log("Attack!");

        if (attackSpeed <= 0f)
            attackSpeed = 0.01f;

        attackCooldown = Mathf.Max(0.01f, 1f / attackSpeed);

        // Scale the attack animation relative to base attack speed
        float normalizedSpeed = Mathf.Max(1f, attackSpeed / 2f);
        animator.SetFloat(ATTACK_SPEED, normalizedSpeed);

        animator.SetTrigger(ATTACK);

        PerformAttack();
    }

    protected abstract void PerformAttack();

    public virtual void UpgradeDamage(float amount)
    {
        damage += amount;
    }

    public virtual void UpgradeAttackSpeed(float amount)
    {
        attackSpeed += amount;
    }

    public void SetVisualActive(bool active)
    {
        isActive = active;

        if (visualSpriteRenderer != null)
            visualSpriteRenderer.enabled = active;

        if (animator != null)
            animator.enabled = active;
    }


}
