using UnityEngine;

public abstract class Weapon : MonoBehaviour
{

    [Header("Position")]
    [SerializeField] private float radius = 1f;

    [Header("Stats")]
    [SerializeField] protected float damage = 10f;
    [SerializeField] protected float attackSpeed = 1f; // attacks per second


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

        // Make weapon face the mouse
        transform.right = dir;
    }

    private bool CanAttack()
    {
        return attackCooldown <= 0f;
    }


    // Public method called by input
    public void Attack()
    {
        if (!CanAttack()) return;

        attackCooldown = Mathf.Max(0.01f, 1f / attackSpeed);

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


}
