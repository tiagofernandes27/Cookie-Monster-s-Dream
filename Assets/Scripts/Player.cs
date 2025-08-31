using UnityEngine;
using static UnityEngine.Rendering.DebugUI;

public class Player : MonoBehaviour
{


    public static Player Instance { get; private set; }


    [Header("Movement")]
    [SerializeField] private float movementSpeed = 5f;
    private Vector2 movementDirection;

    [Header("Dash")]
    [SerializeField] private float dashSpeed = 10f;
    [SerializeField] private float dashDuration = 0.2f;
    [SerializeField] private float dashCooldown = 0.5f;
    [SerializeField] private bool useCooldown = true;
    private Vector2 dashDirection;
    private bool isDashing;
    private bool canDash;
    private float dashTimer;
    private float cooldownTimer;

    [Header("References")]
    private Rigidbody2D playerRigidbody2D;

    private bool isRunning;
    private bool isFacingRight = false;

    private int playerMoney;


    private void Awake()
    {
        Instance = this;

        playerRigidbody2D = GetComponent<Rigidbody2D>();

        canDash = true;
    }

    private void Start()
    {
        GameInput.Instance.OnDashAction += GameInput_OnDashAction;
    }

    private void GameInput_OnDashAction(object sender, System.EventArgs e)
    {
        if (canDash)
        {
            isDashing = true;
            canDash = false;

            dashDirection = movementDirection;

            if (dashDirection == Vector2.zero)
            {
                dashDirection = dashDirection = isFacingRight ? Vector2.right : Vector2.left;
            }

            dashTimer = dashDuration;
        }
    }

    private void FixedUpdate()
    {
        if (isDashing)
        {
            playerRigidbody2D.linearVelocity = dashDirection.normalized * dashSpeed;
            return;
        }

        playerRigidbody2D.linearVelocity = movementDirection * movementSpeed;
    }

    private void Update()
    {
        ProcessInputs();
        HandleDash();
    }

    private void ProcessInputs()
    {
        movementDirection = GameInput.Instance.GetMovementVectorNormalized();

        isRunning = movementDirection != Vector2.zero;

        // Update facing direction
        if (movementDirection.x > 0 & !isFacingRight) isFacingRight = true;
        else if (movementDirection.x < 0 & isFacingRight) isFacingRight = false;
    }

    private void HandleDash()
    {
        if (isDashing)
        {
            dashTimer -= Time.deltaTime;
            if (dashTimer <= 0f)
            {
                isDashing = false;

                if (useCooldown)
                {
                    cooldownTimer = dashCooldown;
                }
                else
                {
                    canDash = true;
                }
            }
        }

        if (!canDash && !isDashing && useCooldown)
        {
            cooldownTimer -= Time.deltaTime;
            if (cooldownTimer <= 0f)
            {
                canDash = true;
            }
        }
    }

    public bool IsRunning()
    {
        return isRunning;
    }

    public bool IsFacingRight()
    {
        return isFacingRight;
    }

    public bool IsDashing()
    {
        return isDashing;
    }

    public int PlayerMoney
    {
        get => playerMoney;
        set => playerMoney = value;
    }


}
