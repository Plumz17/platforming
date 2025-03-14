using System;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private Rigidbody2D rb;
    private float moveInput;
    private bool isGrounded;
    private float coyoteTimeCounter;


    [Header("Movement Settings")]
    public float speed = 10f;
    public float jumpForce = 10f;
    public float lowJumpMultiplier = 1.5f; 
    public float fallMultiplier = 2f; 

    [Header("Ground Settings")]
    public float groundCheckDistance = 1.1f;
    public LayerMask GroundLayer;

    [Header("Coyote Time Settings")]
    [SerializeField] private float coyoteTime = 0.2f;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
    
    }

    // Update is called once per frame
    void Update()
    {
        HandleJumping();
        CheckGround();
        ApplyVariableJump();
    }

    void FixedUpdate()
    {
        HandleMovement();
    }

    private void HandleJumping()
    {
        if (Input.GetKeyDown(KeyCode.Space) && coyoteTimeCounter > 0f)
        {
            Jump();
        }
    }

    private void HandleMovement()
    {
        moveInput = Input.GetAxisRaw("Horizontal");
        rb.linearVelocity = new Vector2(moveInput * speed, rb.linearVelocityY);
    }

    private void CheckGround()
    {
        if (Physics2D.Raycast(transform.position, Vector2.down, groundCheckDistance, GroundLayer))
        {
            isGrounded = true;
            coyoteTimeCounter = coyoteTime;  // Reset coyote time when grounded
        }
        else
        {
            isGrounded = false;
            coyoteTimeCounter -= Time.deltaTime;  // Countdown coyote time when in air
        }
    }

    void ApplyVariableJump()
    {
        if (rb.linearVelocityY < 0)
        {
            // Apply extra gravity when falling
            rb.linearVelocity += Vector2.up * Physics2D.gravity.y * fallMultiplier * Time.deltaTime;
        }
        else if (rb.linearVelocityY > 0 && !Input.GetKey(KeyCode.Space))
        {
            // Apply lower gravity before peak if jump button is released early
            rb.linearVelocity += Vector2.up * Physics2D.gravity.y * lowJumpMultiplier * Time.deltaTime;
        }
    }

    void Jump() {
        rb.linearVelocity = new Vector2(rb.linearVelocityX, jumpForce);
        coyoteTimeCounter = 0f;
    }
}
