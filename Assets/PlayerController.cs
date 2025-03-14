using System;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private Rigidbody2D rb;
    private float moveInput;
    private bool isGrounded;


    [Header("Movement Settings")]
    public float speed = 10f;
    public float jumpForce = 10f;

    [Header("Ground Settings")]
    public float groundCheckDistance = 1.1f;
    public LayerMask GroundLayer;


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
        moveInput = Input.GetAxisRaw("Horizontal");
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded) {
            Jump();
        }
        CheckGround();
    }

    private void CheckGround()
    {
        isGrounded = Physics2D.Raycast(transform.position, Vector2.down, groundCheckDistance, GroundLayer);
    }

    void Jump() {
        rb.linearVelocity = new Vector2(rb.linearVelocityX, jumpForce);
    }

    void FixedUpdate()
    {
        rb.linearVelocity = new Vector2(moveInput * speed, rb.linearVelocityY);
    }
}
