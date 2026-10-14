
using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{
    private PlayerInput inputActions;

    public Vector2 MoveInput{  get; private set; }
    public bool JumpPressed{  get; private set; }
    public bool FirePressed {  get; private set; }

    private bool inputLock = true;

    private bool waitForAttackRelease;

    private void Awake()
    {
        inputActions = new PlayerInput();
    }

    private void Update()
    {
        if (inputLock)
        {
            MoveInput = Vector2.zero;
            JumpPressed = false;
            FirePressed = false;
            return;
        }

        MoveInput = inputActions.Player.Move.ReadValue<Vector2>();
        JumpPressed = inputActions.Player.Jump.WasPressedThisFrame();
        
        if (waitForAttackRelease)
        {
            if (!inputActions.Player.Attack.IsPressed())
            {
                waitForAttackRelease = false;
            }
            FirePressed = false;
            return ;
        }

        FirePressed=inputActions.Player.Attack.IsPressed();
    }

    public void LockInput()
    {
        inputLock = true;

        MoveInput= Vector2.zero;
        JumpPressed = false;
        FirePressed = false;
    }

    public void UnlockInput()
    {
        inputLock = false;

        MoveInput=Vector2.zero;
        JumpPressed = false;
        FirePressed = false;

        waitForAttackRelease=inputActions.Player.Attack.IsPressed();
    }

    private void OnEnable()
    {
        inputActions.Enable();
    }

    private void OnDisable()
    {
        inputActions.Disable();
    }
}
