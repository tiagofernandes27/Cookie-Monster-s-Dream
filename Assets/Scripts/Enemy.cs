using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{


    [Header("General Stats")]
    [SerializeField] private EnemyType enemyType;
    [SerializeField] private int damage = 1;
    [SerializeField] private float speed = 2f;
    [SerializeField] private float health = 5f;
    private float defaultSpeed;

    [Header("Melee Settings")]
    [SerializeField] private RatFist weapon;
    [SerializeField] private float attackCooldown = 1f;
    private bool isAttacking = false;

    [Header("Ranged Settings")]
    [SerializeField] private GameObject projectilePrefab;
    [SerializeField] private Transform firePoint;
    [SerializeField] private float shootCooldown = 2f;

    [Header("Suicide Settings")]
    [SerializeField] private float explodeRadius = 2f;
    [SerializeField] private int explodeDamage = 3;

    [Header("Animation")]
    [SerializeField] private Animator animator;
    [SerializeField] private SpriteRenderer spriteRenderer;

    private Transform player;
    private float cooldownTimer = 0f;
    private bool isWalking;
    private bool isExploding = false;


    private void Start()
    {
        player = Player.Instance.transform;

        defaultSpeed = speed;

        weapon.gameObject.SetActive(false);
    }

    private void Update()
    {
        if (health <= 0) return;

        HandleMovement();
        HandleCombat();
        HandleAnimation();

        if (cooldownTimer > 0f)
            cooldownTimer -= Time.deltaTime;
    }

    #region Movement
    private void HandleMovement()
    {
        if (enemyType == EnemyType.Melee || enemyType == EnemyType.Suicide || enemyType == EnemyType.Ranged)
        {
            Vector2 direction = (player.position - transform.position).normalized;
            transform.position = Vector2.MoveTowards(transform.position, player.position, speed * Time.deltaTime);
            isWalking = direction.magnitude > 0.01f;

            if (spriteRenderer != null)
                spriteRenderer.flipX = direction.x < -0.01f;
        }
    }
    #endregion

    #region Combat
    private void HandleCombat()
    {
        switch (enemyType)
        {
            case EnemyType.Melee:
                if (!isAttacking)
                {
                    if (cooldownTimer <= 0f)
                    {
                        float distancem = Vector2.Distance(transform.position, player.position);
                        if (distancem <= weapon.AttackRange)
                        {
                            StartCoroutine(MeleeAttackCoroutine());


                            cooldownTimer = attackCooldown;
                        }
                    }
                }
                break;

            case EnemyType.Ranged:
                if (cooldownTimer <= 0f)
                {
                    StartCoroutine(ShootAndPauseCoroutine());

                    cooldownTimer = shootCooldown;
                }
                break;

            case EnemyType.Suicide:
                float distance = Vector2.Distance(transform.position, player.position);
                if (distance <= explodeRadius)
                {
                    isExploding = true;
                    if (animator != null)
                        animator.SetBool("IsExploding", true);

                    speed = 0f;
                }
                break;
        }
    }

    private IEnumerator MeleeAttackCoroutine()
    {
        isAttacking = true;

        // Stop movement
        speed = 0f;
        isWalking = false;

        weapon.gameObject.SetActive(true);

        // Store player's current position
        Vector2 targetPosition = player.position;

        // Optional: short wind-up before weapon swings
        yield return new WaitForSeconds(0.2f);

        // Call weapon script to deal damage
        weapon.Swing(damage, targetPosition);

        // Wait remaining cooldown before moving again
        yield return new WaitForSeconds(0.4f);

        weapon.gameObject.SetActive(false);

        // Resume movement
        speed = defaultSpeed;
        isAttacking = false;
        isWalking = true;
    }

    private IEnumerator ShootAndPauseCoroutine()
    {
        // Stop movement
        speed = 0f;
        isWalking = false;

        // Play shoot animation if you have one
        if (animator != null)
            animator.SetTrigger("Shoot");

        yield return new WaitForSeconds(0.3f);

        // Shoot projectile
        ShootProjectile();

        // Wait for 1 second
        yield return new WaitForSeconds(1f);

        // Resume movement
        speed = defaultSpeed;
    }

    private void ShootProjectile()
    {
        if (projectilePrefab == null || firePoint == null) return;

        Vector2 direction = (player.position - firePoint.position).normalized;
        GameObject proj = Instantiate(projectilePrefab, firePoint.position, Quaternion.identity);
        proj.GetComponent<EnemyProjectile>()?.Shoot(direction, damage);
    }

    public void DealExplosionDamage()
    {
        // Damage player if in radius
        if (Vector2.Distance(player.position, transform.position) <= explodeRadius)
        {
            player.GetComponent<PlayerHealth>()?.ChangeHealth(-explodeDamage);
        }

        // Optionally add VFX, sound, etc.
        
    }

    public void Explode()
    {
        Destroy(gameObject);
    }

    public bool IsExploding()
    {
        return isExploding;
    }

    public void TakeDamage(float amount)
    {
        health -= amount;
        if (health <= 0) Die();
    }

    private void Die()
    {
        Destroy(gameObject);
    }
    #endregion

    #region Animation
    private void HandleAnimation()
    {
        if (animator != null)
            animator.SetBool("IsWalking", isWalking);
    }
    #endregion


}

public enum EnemyType
{
    Melee,
    Suicide,
    Ranged
}