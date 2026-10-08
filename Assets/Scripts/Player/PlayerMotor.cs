using UnityEngine;

public class PlayerMotor : MonoBehaviour
{
    private Rigidbody2D rb;
    private SpriteRenderer SpriteRenderer;
    private Animator animator;

    private InputManager inputManager;
    private PlayerStateMachine stateMachine;
    private PlayerAttack playerAttack;

    public Animator Animator => animator;
    public InputManager InputManager => inputManager;
    public PlayerAttack PlayerAttack => playerAttack;
    public PlayerStateMachine StateMachine => stateMachine;
    public Rigidbody2D Rigidbody => rb;

    public bool IsDead { get; set; }
    public bool IsRespawning { get; set; }

    [Header("Player")]
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float jumpForce = 12f;
    [SerializeField] private int maxJumps = 2;

    [Header("Ground Check")]
    [SerializeField] private Transform groundCheck;
    [SerializeField] private float groundRadius = 0.2f;
    [SerializeField] private LayerMask groundLayer;

    [Header("Wall Check")]
    [SerializeField] private Transform wallCheck;
    [SerializeField]
    private Vector2 wallCheckSize =
        new Vector2(0.2f, 1f);

    [SerializeField] private LayerMask wallLayer;
    [SerializeField] private float wallCheckDistance = 0.1f;
    [SerializeField] private float wallSlideSpeed = 2f;

    public bool isGrounded;
    public bool isLeftWall;
    public bool isRightWall;

    public bool isWall =>
        isLeftWall || isRightWall;

    public bool isWallSliding;

    private int jumpCount;
    private int facingDirection = 1;

    private Vector3 wallCheckOriginalLocalPos;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        SpriteRenderer = GetComponent<SpriteRenderer>();
        inputManager = GetComponent<InputManager>();
        animator = GetComponent<Animator>();
        playerAttack = GetComponent<PlayerAttack>();

        stateMachine = new PlayerStateMachine();

        stateMachine.Initialize(
            new IdleState(
                this,
                stateMachine
            )
        );

        wallCheckOriginalLocalPos =
            wallCheck.localPosition;
    }

    private void Update()
    {
        if (IsDead || IsRespawning)
            return;

        CheckGround();
        CheckWall();
        Jump();

        stateMachine.Update();
    }

    private void FixedUpdate()
    {
        if (IsDead || IsRespawning)
            return;

        Move();
        WallSlide();

        stateMachine.FixedUpdate();
    }

    private void CheckGround()
    {
        isGrounded = Physics2D.OverlapCircle(
            groundCheck.position,
            groundRadius,
            groundLayer
        );

        if (!isGrounded)
        {
            bool onWallTop =
                Physics2D.OverlapCircle(
                    groundCheck.position,
                    groundRadius,
                    wallLayer
                );

            if (onWallTop)
                isGrounded = true;
        }

        if (isGrounded)
        {
            jumpCount = 0;

            isLeftWall = false;
            isRightWall = false;
        }
    }

    private void CheckWall()
    {
        if (isGrounded)
        {
            isLeftWall = false;
            isRightWall = false;

            return;
        }

        wallCheck.localPosition = new Vector2(
            Mathf.Abs(
                wallCheckOriginalLocalPos.x
            ) * facingDirection,

            wallCheckOriginalLocalPos.y
        );

        Vector2 direction =
            facingDirection > 0
            ? Vector2.right
            : Vector2.left;

        bool hitWall = Physics2D.BoxCast(
            wallCheck.position,
            wallCheckSize,
            0f,
            direction,
            wallCheckDistance,
            wallLayer
        );

        isRightWall =
            hitWall && facingDirection > 0;

        isLeftWall =
            hitWall && facingDirection < 0;
    }

    public void Move()
    {
        float horizontalInput =
            inputManager.MoveInput.x;

        if (isWallSliding)
        {
            if (isRightWall &&
                horizontalInput > 0)
            {
                horizontalInput = 0;
            }

            if (isLeftWall &&
                horizontalInput < 0)
            {
                horizontalInput = 0;
            }
        }

        rb.linearVelocity = new Vector2(
            horizontalInput * moveSpeed,
            rb.linearVelocity.y
        );

        if (inputManager.MoveInput.x > 0)
        {
            facingDirection = 1;
            SpriteRenderer.flipX = false;
        }
        else if (inputManager.MoveInput.x < 0)
        {
            facingDirection = -1;
            SpriteRenderer.flipX = true;
        }
    }

    private void WallSlide()
    {
        isWallSliding =
            !isGrounded &&
            isWall &&
            rb.linearVelocity.y <= 0;

        if (isWallSliding)
        {
            rb.linearVelocity = new Vector2(
                rb.linearVelocity.x,
                Mathf.Max(
                    rb.linearVelocity.y,
                    -wallSlideSpeed
                )
            );
        }
    }

    public void Jump()
    {
        if (!inputManager.JumpPressed)
            return;

        float horizontalInput =
            inputManager.MoveInput.x;

        if (isGrounded)
        {
            rb.linearVelocity = new Vector2(
                rb.linearVelocity.x,
                jumpForce
            );

            jumpCount = 0;
        }
        else if (isWall)
        {
            if (isRightWall &&
                horizontalInput < 0)
            {
                rb.linearVelocity = new Vector2(
                    rb.linearVelocity.x,
                    jumpForce
                );
            }
            else if (isLeftWall &&
                     horizontalInput > 0)
            {
                rb.linearVelocity = new Vector2(
                    rb.linearVelocity.x,
                    jumpForce
                );
            }
        }
        else if (jumpCount < maxJumps - 1)
        {
            rb.linearVelocity = new Vector2(
                rb.linearVelocity.x,
                jumpForce
            );

            jumpCount++;
        }
    }
}