using UnityEngine;

namespace Game
{
    public class CharacterInput : MonoBehaviour
    {
        [SerializeField] private Character character;


        [SerializeField] private KeyCode
            keyToRun = KeyCode.LeftShift,
            keyToJump = KeyCode.Space,
            keyToGoLeft = KeyCode.A,
            keyToGoRight = KeyCode.D;

        void Update()
        {
            CheckPlayerMoving();

            CheckPlayerRunning();
            CheckPlayerJumping();
        }

        void CheckPlayerJumping()
        {
            if (Input.GetKeyDown(keyToJump))
            {
                character.MovementStarted(CharacterMovementType.Jump);
            }
        }

        void CheckPlayerRunning()
        {
            if (Input.GetKeyDown(keyToRun))
            {
                character.MovementStarted(CharacterMovementType.Run);
            }
            else if (Input.GetKeyUp(keyToRun))
            {
                character.MovementStopped(CharacterMovementType.Run);
            }
        }

        void CheckPlayerMoving()
        {
            if (Input.GetKeyDown(keyToGoLeft))
            {
                character.MovementStarted(CharacterMovementType.MoveLeft);
            }
            else if (Input.GetKeyUp(keyToGoLeft))
            {
                character.MovementStopped(CharacterMovementType.MoveLeft);
            }

            if (Input.GetKeyDown(keyToGoRight))
            {
                character.MovementStarted(CharacterMovementType.MoveRight);
            }
            else if (Input.GetKeyUp(keyToGoRight))
            {
                character.MovementStopped(CharacterMovementType.MoveRight);
            }
        }
    }

}