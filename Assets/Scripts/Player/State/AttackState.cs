using UnityEngine;

public class AttackState : PlayerState
{
    public AttackState(PlayerMotor player, PlayerStateMachine stateMachine)
        : base(player, stateMachine)
    {
    }

    public override void Enter()
    {
        player.Animator.SetTrigger("Attack");
    }

    public override void Update()
    {
        if (player.IsDead)
        {
            stateMachine.ChangeState(new DeathState(player, stateMachine));
            return;
        }

        if (player.isGrounded)
        {
            stateMachine.ChangeState(new IdleState(player, stateMachine));
        }
    }

    public override void FixedUpdate()
    {
    }

    public override void Exit()
    {
    }
}