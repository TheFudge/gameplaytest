using UnityEngine;

namespace Game
{
    public class Character : MonoBehaviour
    {
        [Header("Character References")]
        [SerializeField] private Animator characterAnimator = null;
        [SerializeField] private Rigidbody2D characterRigidBody = null;
        [SerializeField] private LayerMask groundHitLayerMask = 0;

        [Header("Movement Settings")]
        [SerializeField] private float moveSpeed = 1f;
        [SerializeField] private float runSpeed = 2f;
        [SerializeField] private float jumpForce = 30f;
        [SerializeField] private float maximumGroundedHeight = 0.1f;

        [SerializeField] private bool stopSlideWhenIdle = true;
        [SerializeField] private bool canControlMovementInAir = false;
        [SerializeField] private bool canRunWhenNotGrounded = false;
        [SerializeField] private bool canJumpWhenNotGrounded = false;
        
        
        [Header("Smooth Movement")]
        [SerializeField] private bool smoothMovementChange = false;
        [SerializeField] private float movementChangeSpeed = 2f;

        private CharacterMoveSpeedType moveType = CharacterMoveSpeedType.Standing;
        // This would probably be useful to other scripts, so I decided to make it public.
        public CharacterMoveSpeedType MoveType => moveType;
        
        private Vector2 currentMovement = Vector2.zero;
        private int movementDirection = 0;
        private bool isGrounded = false;
        
        private static readonly int animationKeySpeed = Animator.StringToHash("Speed");
        private static readonly int animationKeyDirection = Animator.StringToHash("Direction");
        private static readonly int animationKeyVerticalVelocity = Animator.StringToHash("VerticalVelocity");

        void Update()
        {
            CheckGrounded();
            
            CalculateNewSpeed();
            
            if (movementDirection != 0 || stopSlideWhenIdle)
            {
                // stop slide will override the velocity value even when the player does not move.
                DoMove();
            }
            
            UpdateAnimation();
        }

        private void CalculateNewSpeed()
        {
            // leave the movement if player can not control the movement while in the air.
            if (!isGrounded && !canControlMovementInAir)
                return;

            float newSpeed = 0f;

            switch (moveType)
            {
                case CharacterMoveSpeedType.Standing:
                    newSpeed = 0f;
                    break;
                case CharacterMoveSpeedType.Walk:
                    newSpeed = moveSpeed * movementDirection;
                    break;
                case CharacterMoveSpeedType.Run:
                    if (!isGrounded && canRunWhenNotGrounded || isGrounded)
                    {
                        newSpeed = runSpeed * movementDirection;
                    }
                    break;
            }
            
            if (smoothMovementChange)
            {
                currentMovement.x = Mathf.Lerp(currentMovement.x , newSpeed, movementChangeSpeed * Time.deltaTime);
            }
            else
            {
                currentMovement.x = newSpeed;
            }
        }

        
        private void DoJump()
        {
            if (!canJumpWhenNotGrounded && !isGrounded)
                return;

            characterRigidBody.AddForce(Vector2.up * jumpForce);
        }

        private void DoMove()
        {
            // move character to left or right with the specified speed
            Vector2 velocity = characterRigidBody.velocity;
            velocity.x = currentMovement.x;
            characterRigidBody.velocity = velocity;
        }

        private void UpdateAnimation()
        {
            characterAnimator.SetFloat(animationKeySpeed, Mathf.Abs(GetAnimatorSpeed()));
            characterAnimator.SetFloat(animationKeyDirection, GetAnimatorSpeed());
            // character should never fall or jump (in the animation) if the character is grounded.
            characterAnimator.SetFloat(animationKeyVerticalVelocity, isGrounded ? 0f : characterRigidBody.velocity.y);
        }

        private void CheckGrounded()
        {
            RaycastHit2D hitEvent = Physics2D.Raycast(transform.position, Vector2.down, maximumGroundedHeight, groundHitLayerMask);
            // check to zero seems to be pretty accurate and ist faster than a collider zero check.
            // if value is not exactly zero, than there will be a hit.
            if (hitEvent.distance == 0f) 
            {
                isGrounded = false;
            }
            else
            {
                isGrounded = true;
            }
        }

        public void MovementStarted(CharacterMoveActionType moveActionType)
        {
            switch (moveActionType)
            {
                case CharacterMoveActionType.MoveLeft:
                    if (moveType == CharacterMoveSpeedType.Standing)
                        moveType = CharacterMoveSpeedType.Walk;
                    movementDirection = -1;
                    break;
                case CharacterMoveActionType.MoveRight:
                    if (moveType == CharacterMoveSpeedType.Standing)
                        moveType = CharacterMoveSpeedType.Walk;
                    movementDirection = 1;
                    break;
                case CharacterMoveActionType.Run:
                    moveType = CharacterMoveSpeedType.Run;
                    break;
                case CharacterMoveActionType.Jump:
                    DoJump();
                    break;
            }
        }

        public void MovementStopped(CharacterMoveActionType moveActionType)
        {
            switch (moveActionType)
            {
                case CharacterMoveActionType.MoveLeft:
                    if (movementDirection == -1)
                    {
                        movementDirection = 0;
                        moveType = CharacterMoveSpeedType.Standing;
                    }
                    break;
                case CharacterMoveActionType.MoveRight:
                    if (movementDirection == 1)
                    {
                        movementDirection = 0;
                        moveType = CharacterMoveSpeedType.Standing;
                    }

                    break;
                case CharacterMoveActionType.Run:
                    if (movementDirection != 0)
                        moveType = CharacterMoveSpeedType.Walk;    
                    else 
                        moveType = CharacterMoveSpeedType.Standing;
                    
                    break;
            }
        }
        
        private float GetAnimatorSpeed()
        {
            switch (moveType)
            {
                case CharacterMoveSpeedType.Standing:
                case CharacterMoveSpeedType.Walk:
                    return movementDirection;
                case CharacterMoveSpeedType.Run:
                    return 2f * movementDirection;
                default:
                    return 0f;
            }
        }
    }
}