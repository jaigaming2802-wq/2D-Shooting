using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{
    private PlayerInput inputActions;

    public Vector2 MoveInput { get; private set; }
    public bool JumpPressed { get; private set; }
    public bool FirePressed { get; private set; }

    private bool inputLocked;

    private void Awake()
    {
        inputActions = new PlayerInput();
    }

    public void Update()
    {
        if (inputLocked)
        {
            MoveInput = Vector2.zero;
            JumpPressed = false;
            FirePressed = false;

            return;
        }

        MoveInput =
            inputActions.Player.Move.ReadValue<Vector2>();

        JumpPressed =
            inputActions.Player.Jump.WasPressedThisFrame();

        FirePressed =
            inputActions.Player.Attack.WasPressedThisFrame();
    }

    public void LockInput()
    {
        inputLocked = true;

        MoveInput = Vector2.zero;
        JumpPressed = false;
        FirePressed = false;
    }

    public void UnlockInput()
    {
        inputLocked = false;

        MoveInput = Vector2.zero;
        JumpPressed = false;
        FirePressed = false;
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