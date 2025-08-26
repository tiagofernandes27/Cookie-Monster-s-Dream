using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{


    public static Player Instance { get; private set; }


    [SerializeField] private float speed = 5f;


    private Rigidbody2D playerRigidbody2D;

    private Vector2 moveDirection;
    private bool isRunning;
    private bool isFacingRight = false;


    private void Awake()
    {
        Instance = this;

        playerRigidbody2D = GetComponent<Rigidbody2D>();
    }

    private void Start()
    {
        
    }

    private void FixedUpdate()
    {
        playerRigidbody2D.linearVelocity = new Vector2(moveDirection.x, moveDirection.y) * speed;
    }

    private void Update()
    {
        ProcessInputs();
    }

    private void ProcessInputs()
    {
        moveDirection = GameInput.Instance.GetMovementVectorNormalized();

        isRunning = moveDirection != Vector2.zero;

        // Update facing direction
        if (moveDirection.x > 0 & !isFacingRight) isFacingRight = true;
        else if (moveDirection.x < 0 & isFacingRight) isFacingRight = false;
    }

    public bool IsRunning()
    {
        return isRunning;
    }

    public bool IsFacingRight()
    {
        return isFacingRight;
    }


}
