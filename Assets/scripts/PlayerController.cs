using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    private Rigidbody2D rb;
    private PlayerInputActions inputActions;
    private Vector2 moveInput;
    private bool isJumpPressed;
    private bool isDashPressed;

    [Header("Movement Settings")]
    public float moveSpeed = 5f;
    public float jumpForce = 10f;
    public float dashForce = 20f;
    public float dashDuration = 0.2f;

    [Header("Ground Check Settings")]
    public Transform groundCheck;
    public float groundCheckRadius = 0.2f;
    public LayerMask groundLayer;
    public float coyoteTime = 0.2f;
    private float coyoteTimeCounter;

    private bool isGrounded;
    private bool isDashing = false;
    private float dashTimer = 0f;
    private bool facingRight = true;

    private int comboStep = 0;
    private float lastAttackTime = 0f;
    private float comboResetTime = 0.8f;
    private Animator animator;
    private bool isJumping;
    private bool isFalling;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        inputActions = new PlayerInputActions();

        inputActions.Player.Move.performed += ctx => moveInput = ctx.ReadValue<Vector2>();
        inputActions.Player.Move.canceled += ctx => moveInput = Vector2.zero;
        inputActions.Player.Jump.performed += ctx => isJumpPressed = true;
        inputActions.Player.Dash.performed += ctx => isDashPressed = true;
        inputActions.Player.Attack.performed += ctx => Attack();
    }

    private void OnEnable()
    {
        inputActions.Player.Enable();
    }

    private void OnDisable()
    {
        inputActions.Player.Disable();
    }

    private void Update()
    {
        HandleFlip();
        CheckGrounded();

        animator.SetFloat("Speed", Mathf.Abs(moveInput.x));

        if (!isGrounded && rb.velocity.y > 0.1f)
        {
            isJumping = true;
            isFalling = false;
        }
        else if (!isGrounded && rb.velocity.y < -0.1f)
        {
            isJumping = false;
            isFalling = true;
        }
        else if (isGrounded)
        {
            isJumping = false;
            isFalling = false;
        }

        animator.SetBool("isJumping", isJumping);
        animator.SetBool("isFalling", isFalling);
    }

    private void FixedUpdate()
    {
        if (isDashing)
        {
            rb.velocity = new Vector2(transform.localScale.x * dashForce, rb.velocity.y);
            dashTimer -= Time.fixedDeltaTime;
            if (dashTimer <= 0)
            {
                isDashing = false;
            }
            return;
        }

        rb.velocity = new Vector2(moveInput.x * moveSpeed, rb.velocity.y);

        if (isJumpPressed)
        {
            Jump();
        }

        if (isDashPressed)
        {
            Dash();
        }
    }

    private void Jump()
    {
        if (coyoteTimeCounter > 0f)
        {
            rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
            coyoteTimeCounter = 0f;
        }
        isJumpPressed = false;
    }

    private void Dash()
    {
        isDashing = true;
        dashTimer = dashDuration;
        isDashPressed = false;
    }

    private void Attack()
    {
        if (Time.time - lastAttackTime > comboResetTime)
        {
            comboStep = 0;
        }

        comboStep++;
        lastAttackTime = Time.time;

        if (comboStep == 1)
        {
            Debug.Log("Attack 1");
        }
        else if (comboStep == 2)
        {
            Debug.Log("Attack 2");
        }
        else if (comboStep == 3)
        {
            Debug.Log("Attack 3");
            comboStep = 0;
        }
        else
        {
            comboStep = 0;
        }
    }

    private void HandleFlip()
    {
        if (moveInput.x > 0 && !facingRight)
        {
            Flip();
        }
        else if (moveInput.x < 0 && facingRight)
        {
            Flip();
        }
    }

    private void Flip()
    {
        facingRight = !facingRight;
        Vector3 scaler = transform.localScale;
        scaler.x *= -1;
        transform.localScale = scaler;
    }

    private void CheckGrounded()
    {
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);

        if (isGrounded)
        {
            coyoteTimeCounter = coyoteTime;
        }
        else
        {
            coyoteTimeCounter -= Time.deltaTime;
        }
    }

    private void OnDrawGizmosSelected()
    {
        if (groundCheck != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(groundCheck.position, groundCheckRadius);
        }
    }
}
