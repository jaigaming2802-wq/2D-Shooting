using UnityEngine;

public class JumpState : PlayerState
{
    public JumpState(PlayerMotor player, PlayerStateMachine stateMachine)
        : base(player, stateMachine)
    {
    }

    public override void Enter()
    {
        player.Animator.SetBool("isJumping", true);
        player.Jump();
    }

    public override void Update()
    {
        if (player.IsDead)
        {
            stateMachine.ChangeState(new DeathState(player, stateMachine));
            return;
        }

        AnimatorStateInfo stateInfo = player.Animator.GetCurrentAnimatorStateInfo(0);

        if (player.isGrounded && stateInfo.normalizedTime >= 1f)
        {
            stateMachine.ChangeState(new IdleState(player, stateMachine));
        }
    }

    public override void FixedUpdate()
    {
    }

    public override void Exit()
    {
        player.Animator.SetBool("isJumping", false);
    }
}