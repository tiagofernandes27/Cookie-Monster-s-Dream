using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerTest : MonoBehaviour
{
    [SerializeField] private float speed = 5f;


    private Rigidbody2D playerRigidbody2D;
    private Vector2 moveDirection;


    private void Awake()
    {
        playerRigidbody2D = GetComponent<Rigidbody2D>();
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
        moveDirection = Vector2.zero;

        if (Keyboard.current.wKey.isPressed) moveDirection.y += 1;
        if (Keyboard.current.sKey.isPressed) moveDirection.y -= 1;
        if (Keyboard.current.dKey.isPressed) moveDirection.x += 1;
        if (Keyboard.current.aKey.isPressed) moveDirection.x -= 1;

        moveDirection = moveDirection.normalized;
    }
}
