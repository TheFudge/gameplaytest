using UnityEngine;
using UnityEngine.Analytics;

public class Character : MonoBehaviour
{
    [SerializeField]
    private Animator animator;
    [SerializeField]
    private Rigidbody2D rigidbody;

    public enum PlayerMovementType
    {
        MoveLeft, MoveRight, Run, Jump
    }

    public int movementDirection = 0;
    private Vector2 currentMovement = Vector2.zero;
    public bool isRunning = false;
    public bool isGrounded = false;

    private AnimationCurve jumpBehaviour;
    private float jumpTime = 0f;
    
    [SerializeField] private float moveSpeed = 1f, runSpeed = 2f, jumpForce = 30f;
    [SerializeField] private float minimumFallingVelocity = 0.1f;
    [SerializeField] private float maximumGroundedHeight = 0.1f;


    [SerializeField] private bool stopSlideWhenIdle = true;
    [SerializeField] private bool canControlMovementInAir = false;
    [SerializeField] private bool canRunWhenNotGrounded = false;
    [SerializeField] private bool canJumpWhenNotGrounded = false;

    [SerializeField] private LayerMask worldLayer;
    
    void Update()
    {
        CheckGrounded();
        
        if (movementDirection != 0 || stopSlideWhenIdle)
        {
            // stop slide will override the velocity value even when the player does not move.
            DoMove();
        }
    }

    private void CalculateNewSpeed()
    {
        // leave the movenet if player can not control the movement while in the air.
        if (!isGrounded && !canControlMovementInAir)
            return;
        
        if (isRunning && ((!isGrounded && canRunWhenNotGrounded) || isGrounded))
        {
            currentMovement.x = runSpeed * movementDirection;
        }
        else
        {
            currentMovement.x = moveSpeed * movementDirection;
        }
    }

    private float GetAnimatorSpeed()
    {
        if (isRunning)
        {
            return 2f * movementDirection;
        }
        else
        {
            return movementDirection;
        }
    }

    private void DoJump()
    {
        if (!canJumpWhenNotGrounded && !isGrounded)
            return;
        
        rigidbody.AddForce(Vector2.up * jumpForce);
        animator.SetTrigger("DoJump");
    }

    private void DoMove()
    {
        // move character to left or right with the specified speed
        Vector2 velocity = rigidbody.velocity;
        velocity.x = currentMovement.x;
        rigidbody.velocity = velocity;

        // set animations to make a cooler character.
        animator.SetFloat("Speed", GetAnimatorSpeed());
        animator.SetFloat("VerticalVelocity", rigidbody.velocity.y);
    }

    private void CheckGrounded()
    {
        RaycastHit2D hitEvent = Physics2D.Raycast(transform.position, Vector2.down, maximumGroundedHeight, worldLayer);
        if (hitEvent.collider == null)
        {
            isGrounded = false;
        }
        else
        {
            isGrounded = true;
        }
    }

    public void MovementStarted(PlayerMovementType movementType)
    {
        switch (movementType)
        {
            case PlayerMovementType.MoveLeft:
                movementDirection = -1;
                break;
            case PlayerMovementType.MoveRight:
                movementDirection = 1;
                break;
            case PlayerMovementType.Run:
                isRunning = true;
                break;
            case PlayerMovementType.Jump:
                DoJump();
                break;
        }

        CalculateNewSpeed();
    }
    
    public void MovementStopped(PlayerMovementType movementType)
    {
        switch (movementType)
        {
            case PlayerMovementType.MoveLeft:
                if (movementDirection == -1)
                    movementDirection = 0;
                break;
            case PlayerMovementType.MoveRight:
                if (movementDirection == 1)
                    movementDirection = 0;
                break;
            case PlayerMovementType.Run:
                isRunning = false;
                break;
        }
        CalculateNewSpeed();
    }
}
