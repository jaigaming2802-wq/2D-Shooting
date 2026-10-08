public abstract class PlayerState
{
    protected PlayerMotor player;
    protected PlayerStateMachine stateMachine;

    public PlayerState(PlayerMotor player, PlayerStateMachine stateMachine)
    {
        this.player = player;
        this.stateMachine = stateMachine;
    }

    public virtual void Enter()
    {

    }

    public virtual void Exit() 
    {

    }

    public virtual void Update()
    {

    }

    public virtual void FixedUpdate()
    {

    }
}