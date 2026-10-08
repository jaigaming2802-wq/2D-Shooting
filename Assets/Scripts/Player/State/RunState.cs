using UnityEngine;

public class RunState : PlayerState
{
    public RunState(PlayerMotor player, PlayerStateMachine stateMachine)
        : base(player, stateMachine)
    {
    }

    public override void Enter()
    {
        player.Animator.SetFloat("Speed", 1f);
    }

    public override void Update()
    {
        if (player.IsDead)
        {
            stateMachine.ChangeState(new DeathState(player, stateMachine));
        }
        else if (player.InputManager.JumpPressed)
        {
            stateMachine.ChangeState(new JumpState(player, stateMachine));
        }
        else if (player.InputManager.MoveInput.x == 0f)
        {
            stateMachine.ChangeState(new IdleState(player, stateMachine));
        }
    }

    public override void Exit()
    {
    }
}