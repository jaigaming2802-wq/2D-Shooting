using UnityEngine;

public class IdleState : PlayerState
{
    public IdleState(PlayerMotor player, PlayerStateMachine stateMachine)
        : base(player, stateMachine)
    {
    }

    public override void Enter()
    {
        player.Animator.SetFloat("Speed", 0f);
    }

    public override void Update()
    {
        if (player.IsDead)
        {
            stateMachine.ChangeState(new DeathState(player, stateMachine));
        }
        else if (player.InputManager.FirePressed)
        {
            stateMachine.ChangeState(new AttackState(player, stateMachine));
        }
        else if (player.InputManager.JumpPressed)
        {
            stateMachine.ChangeState(new JumpState(player, stateMachine));
        }
        else if (player.InputManager.MoveInput.x != 0 && player.isGrounded)
        {
            stateMachine.ChangeState(new RunState(player, stateMachine));
        }
    }

    public override void Exit()
    {
    }
}