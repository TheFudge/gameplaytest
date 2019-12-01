using UnityEngine;

public class CharacterInput : MonoBehaviour
{
    [SerializeField]
    private Character character;


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
            character.MovementStarted(Character.PlayerMovementType.Jump);
        }
    }

    void CheckPlayerRunning()
    {
        if (Input.GetKeyDown(keyToRun))
        {
            character.MovementStarted(Character.PlayerMovementType.Run);
        }
        else if(Input.GetKeyUp(keyToRun))
        {
            character.MovementStopped(Character.PlayerMovementType.Run);   
        }
    }

    void CheckPlayerMoving()
    {
        if (Input.GetKeyDown(keyToGoLeft))
        {
            character.MovementStarted(Character.PlayerMovementType.MoveLeft);
        }
        else if(Input.GetKeyUp(keyToGoLeft))
        {
            character.MovementStopped(Character.PlayerMovementType.MoveLeft);
        }
        if (Input.GetKeyDown(keyToGoRight))
        {
            character.MovementStarted(Character.PlayerMovementType.MoveRight);
        }
        else if(Input.GetKeyUp(keyToGoRight))
        {
            character.MovementStopped(Character.PlayerMovementType.MoveRight);
        }
    }
}
