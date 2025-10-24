using UnityEngine;


[RequireComponent(typeof(Rigidbody2D))]
public class PlayerController : MonoBehaviour
{
    [Header("Movement Settings")]
    public float moveSpeed = 5.0f;

    private Rigidbody2D rb;
    private Vector2 moveInput;
    private Vector2 lastMoveDirection;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        // Get input from keyboard
        moveInput.x = Input.GetAxisRaw("Horizontal");
        moveInput.y = Input.GetAxisRaw("Vertical");
        
        // Normalize the input vector to prevent faster diagonal movement
        moveInput = moveInput.normalized; 

        // Store the last move direction if there is input
        if(moveInput != Vector2.zero)
        {
            lastMoveDirection = moveInput;
        }
    }

    void FixedUpdate()
    {
        // Move the player based on input
        rb.MovePosition(rb.position + moveInput * moveSpeed * Time.fixedDeltaTime);
        
        if(moveInput.sqrMagnitude > 0.01f)
            transform.up = moveInput;
    }
}
