using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    private Rigidbody2D rb;
    private PlayerInputActions inputActions;
    private Vector2 moveInput;
    private bool isJumpPressed;
    private bool isDashPressed;
    private bool isAttacking;

    [Header("Movement Settings")]
    public float moveSpeed = 5f;
    public float jumpForce = 10f;
    public float dashForce = 20f;
    public float dashDuration = 0.2f;

    private bool isDashing = false;
    private float dashTimer = 0f;
    private bool facingRight = true;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
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
        rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
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
        if (isAttacking) return;
        StartCoroutine(AttackCombo());
    }

    private System.Collections.IEnumerator AttackCombo()
    {
        isAttacking = true;
        for (int i = 0; i < 3; i++)
        {
            Debug.Log($"Attack {i + 1}");
            yield return new WaitForSeconds(0.3f); 
        }
        isAttacking = false;
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
}

