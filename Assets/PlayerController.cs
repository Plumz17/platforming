//Added Features:
// Sideways Movement
// Jumping
// Ground Check
// Variable Jump Height
// Coyote Time
// Apex Modifier
// Jump Buffering
// Clamped Fall Speed
// (Edge Detection looks difficult, got no time for that)
// Github page: https://github.com/Plumz17/platforming

using System;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private Rigidbody2D rb;
    private float moveInput;
    private bool isGrounded;
    private float coyoteTimeCounter;
    private float jumpBufferCounter;

    [Header("Movement Settings")]
    public float speed = 10f;
    public float jumpForce = 10f;
    public float lowJumpMultiplier = 1.5f; 
    public float fallMultiplier = 2f; 
    public float maxFallSpeed = 20f; 

    [Header("Ground Settings")]
    public float groundCheckDistance = 1.1f;
    public LayerMask GroundLayer;

    [Header("Coyote Time Settings")]
    [SerializeField] private float coyoteTime = 0.2f;
    [SerializeField] private float jumpBufferTime = 0.25f;


    void Awake() {
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update() {
        HandleJumping();
        CheckGround();
        ApplyVariableJump();
        Debug.Log(rb.linearVelocityY);
    }

    void FixedUpdate() {
        HandleMovement();
    }

    private void HandleJumping() {
        if (Input.GetKeyDown(KeyCode.Space)) {
            jumpBufferCounter = jumpBufferTime;
        }
        else {
            jumpBufferCounter -= Time.deltaTime;
        }

        if (jumpBufferCounter > 0f && coyoteTimeCounter > 0f) {
            Jump();
        }
    }

    private void HandleMovement() {
        moveInput = Input.GetAxisRaw("Horizontal");
        rb.linearVelocity = new Vector2(moveInput * speed, rb.linearVelocityY);
    }

    private void CheckGround()
    {
        if (Physics2D.Raycast(transform.position, Vector2.down, groundCheckDistance, GroundLayer)) {
            if (!isGrounded && jumpBufferCounter > 0f) {
                Jump(); 
            }
            isGrounded = true;
            coyoteTimeCounter = coyoteTime;  
        }
        else {
            isGrounded = false;
            coyoteTimeCounter -= Time.deltaTime; 
        }
    }

    void ApplyVariableJump() {
        if (rb.linearVelocityY < 0) {
            // Apply extra gravity when falling
            rb.linearVelocity += Vector2.up * Physics2D.gravity.y * fallMultiplier * Time.deltaTime;
        }
        else if (rb.linearVelocityY > 0 && !Input.GetKey(KeyCode.Space)) {
            // Apply lower gravity before peak if jump button is released early
            rb.linearVelocity += Vector2.up * Physics2D.gravity.y * lowJumpMultiplier * Time.deltaTime;
        }

        if (rb.linearVelocityY < -maxFallSpeed) {
            rb.linearVelocity = new Vector2(rb.linearVelocityX, -maxFallSpeed);
        }
    }

    void Jump() {
        rb.linearVelocity = new Vector2(rb.linearVelocityX, jumpForce);
        coyoteTimeCounter = 0f;
        jumpBufferCounter = 0f;
    }
}