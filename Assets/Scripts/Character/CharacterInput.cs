using UnityEngine;

namespace Game
{
    public class CharacterInput : MonoBehaviour
    {
        [SerializeField] private Character character = null;

        [SerializeField] private KeyCode
            keyToRun = KeyCode.LeftShift,
            keyToJump = KeyCode.Space,
            keyToGoLeft = KeyCode.A,
            keyToGoRight = KeyCode.D;

        void Update()
        {
            // Why seperate class?
            // Input code should not be in character class, because we could make a game controller input class and enable the input classes on runtime without destroying readablility of the character class:
            
            CheckPlayerMoving();

            CheckPlayerRunning();
            CheckPlayerJumping();
        }

        void CheckPlayerJumping()
        {
            if (Input.GetKeyDown(keyToJump))
            {
                character.MovementStarted(CharacterMoveActionType.Jump);
            }
        }

        void CheckPlayerRunning()
        {
            if (Input.GetKeyDown(keyToRun))
            {
                character.MovementStarted(CharacterMoveActionType.Run);
            }
            else if (Input.GetKeyUp(keyToRun))
            {
                character.MovementStopped(CharacterMoveActionType.Run);
            }
        }

        void CheckPlayerMoving()
        {
            if (Input.GetKeyDown(keyToGoLeft))
            {
                character.MovementStarted(CharacterMoveActionType.MoveLeft);
            }
            else if (Input.GetKeyUp(keyToGoLeft))
            {
                character.MovementStopped(CharacterMoveActionType.MoveLeft);
            }

            if (Input.GetKeyDown(keyToGoRight))
            {
                character.MovementStarted(CharacterMoveActionType.MoveRight);
            }
            else if (Input.GetKeyUp(keyToGoRight))
            {
                character.MovementStopped(CharacterMoveActionType.MoveRight);
            }
        }
    }

}