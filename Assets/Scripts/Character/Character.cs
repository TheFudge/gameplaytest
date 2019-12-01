using UnityEngine;

namespace Game
{
    public class Character : MonoBehaviour
    {
        [SerializeField] private Animator characterAnimator;
        [SerializeField] private Rigidbody2D characterRigidBody;

        public int movementDirection = 0;
        private Vector2 currentMovement = Vector2.zero;
        public bool isRunning = false;
        public bool isGrounded = false;

        private AnimationCurve jumpBehaviour;
        private float jumpTime = 0f;

        [SerializeField] private float moveSpeed = 1f, runSpeed = 2f, jumpForce = 30f;
        [SerializeField] private float maximumGroundedHeight = 0.1f;


        [SerializeField] private bool stopSlideWhenIdle = true;
        [SerializeField] private bool canControlMovementInAir = false;
        [SerializeField] private bool canRunWhenNotGrounded = false;
        [SerializeField] private bool canJumpWhenNotGrounded = false;
        [SerializeField] private bool smoothMovementChange = false;
        [SerializeField] private float movementChangeSpeed = 2f;

        [SerializeField] private LayerMask worldLayer;
        
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
        }

        private void CalculateNewSpeed()
        {
            // leave the movenet if player can not control the movement while in the air.
            if (!isGrounded && !canControlMovementInAir)
                return;

            float newSpeed = 0f;
            
            if (isRunning && ((!isGrounded && canRunWhenNotGrounded) || isGrounded))
            {
                newSpeed = runSpeed * movementDirection;
            }
            else
            {
                newSpeed = moveSpeed * movementDirection;
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

            characterRigidBody.AddForce(Vector2.up * jumpForce);
        }

        private void DoMove()
        {
           
            // move character to left or right with the specified speed
            Vector2 velocity = characterRigidBody.velocity;
            velocity.x = currentMovement.x;
            characterRigidBody.velocity = velocity;

            // set animations to make a cooler character.
            characterAnimator.SetFloat(animationKeySpeed, Mathf.Abs(GetAnimatorSpeed()));
            characterAnimator.SetFloat(animationKeyDirection, GetAnimatorSpeed());
            // character should never fall or jump (in the animation) if the character is grounded.
            characterAnimator.SetFloat(animationKeyVerticalVelocity, isGrounded ? 0f : characterRigidBody.velocity.y);
        }

        private void CheckGrounded()
        {
            RaycastHit2D hitEvent =
                Physics2D.Raycast(transform.position, Vector2.down, maximumGroundedHeight, worldLayer);
            if (hitEvent.collider == null)
            {
                isGrounded = false;
            }
            else
            {
                isGrounded = true;
            }
        }

        public void MovementStarted(CharacterMovementType movementType)
        {
            switch (movementType)
            {
                case CharacterMovementType.MoveLeft:
                    movementDirection = -1;
                    break;
                case CharacterMovementType.MoveRight:
                    movementDirection = 1;
                    break;
                case CharacterMovementType.Run:
                    isRunning = true;
                    break;
                case CharacterMovementType.Jump:
                    DoJump();
                    break;
            }
        }

        public void MovementStopped(CharacterMovementType movementType)
        {
            switch (movementType)
            {
                case CharacterMovementType.MoveLeft:
                    if (movementDirection == -1)
                        movementDirection = 0;
                    break;
                case CharacterMovementType.MoveRight:
                    if (movementDirection == 1)
                        movementDirection = 0;
                    break;
                case CharacterMovementType.Run:
                    isRunning = false;
                    break;
            }
        }
    }
}