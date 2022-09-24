using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float speed = 5;
    public float slidingSpeed;
    public float jumpForce = 4;
    public Transform groundCheck;
    public float checkGroundRadius;
    public LayerMask groundLayer;
    public LayerMask wallLayer;
    public Transform wallCheck;
    public float wallCheckDistance;
    public float jumpOffWallSpeed = 30;

    Animator animator;
    SpriteRenderer spriteRenderer;
    RaycastHit2D wallCheckHitRight;
    RaycastHit2D wallCheckHitLeft;
    bool isRunning;
    bool canJumpOffWall = false;
    bool canJump;
    float moveBy;
    int possibleJumps = 2;
    bool isWallSliding = false;
    bool isGrounded = true;
    Rigidbody2D rb;
    PlayerAnimation playerAnimation;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        playerAnimation = GetComponent<PlayerAnimation>();
    }

    // Update is called once per frame
    void Update()
    {
        float x = Input.GetAxisRaw("Horizontal");

        moveBy = x * speed;
        
        if (moveBy > 0)
        {
            spriteRenderer.flipX = false;

        }
        else if (moveBy < 0)
        {
            spriteRenderer.flipX = true;
        }

        isGrounded = Physics2D.OverlapCircle(groundCheck.position, checkGroundRadius, groundLayer);

        if (!isGrounded)
        {
            animator.SetBool("run", false);
            animator.SetFloat("jumpMode", rb.velocity.y);

            if (!isWallSliding)
            {
                animator.SetBool("isJumping", true);
                animator.SetBool("slideWall", false);
            }
                
        }
        else
        {
            animator.SetBool("slideWall", false);
            animator.SetBool("isJumping", false);
            if (moveBy != 0)
            {
                animator.SetBool("run", true);
            }
            else
            {
                animator.SetBool("run", false);
            }
        }        

        if (isGrounded && possibleJumps < 2)
        {
            possibleJumps = 2;
            playerAnimation.EmitDustOnBothSides();
        }

        if (!isGrounded && possibleJumps > 1 && !canJump)
        {
            possibleJumps = 1;
        }

        wallCheckHitRight = Physics2D.Raycast(wallCheck.position, Vector3.right, wallCheckDistance, wallLayer);

        wallCheckHitLeft = Physics2D.Raycast(wallCheck.position, Vector3.left, wallCheckDistance, wallLayer);

        if ((wallCheckHitRight || wallCheckHitLeft) && rb.velocity.y < 0 && !isGrounded)
        {
            if (!isWallSliding)
            {
                if (wallCheckHitRight)
                {
                    playerAnimation.EmitBottomLeftParticle();
                }
                else if (wallCheckHitLeft)
                {
                    playerAnimation.EmitBottomRightParticle();
                }
            }
            

            possibleJumps = 2;
            isWallSliding = true;
        }
        else
        {
            isWallSliding = false;
        }

        if (possibleJumps > 0)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                if (isWallSliding)
                {
                    canJumpOffWall = true;
                       
                }
                else
                {
                    canJump = true;
                }
            }

        }
    }

    void FixedUpdate()
    { 

        rb.velocity = new Vector2(moveBy, rb.velocity.y);

        if (canJump)
        {
            if (possibleJumps == 1)
            {
                animator.SetTrigger("doubleJump");
                animator.SetBool("slideWall", false);
                animator.SetBool("isJumping", true);
            }

            playerAnimation.EmitBottomDust();

            canJump = false;
            --possibleJumps;
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
        }
        

        if (isWallSliding)
        {
            if (rb.velocity.y < -slidingSpeed)
            {
                rb.velocity = new Vector2(rb.velocity.x, -slidingSpeed);
                animator.SetBool("isJumping", false);
                animator.SetBool("slideWall", true);
            }
        }

        if (canJumpOffWall)
        {
            canJumpOffWall = false;
            
            isWallSliding = false;

            --possibleJumps;

            animator.SetBool("slideWall", false);
            animator.SetBool("isJumping", true);
            animator.SetTrigger("doubleJump");

            if (wallCheckHitRight)
            {
                rb.velocity = new Vector2(-jumpOffWallSpeed, jumpForce);

            } else if (wallCheckHitLeft)
            {
                rb.velocity = new Vector2(jumpOffWallSpeed, jumpForce);
            }
        }

        
    }
}
