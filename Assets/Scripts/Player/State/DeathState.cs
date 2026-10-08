using UnityEngine;

public class DeathState : PlayerState
{
    public DeathState(
        PlayerMotor player,
        PlayerStateMachine stateMachine)
        : base(player, stateMachine)
    {
    }

    public override void Enter()
    {
        player.Animator.SetTrigger("Death");
    }

    public override void Update()
    {
    }

    public override void FixedUpdate()
    {
    }

    public override void Exit()
    {
    }
}