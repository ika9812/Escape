using UnityEngine;

public class IdleState : IPlayerState
{
    Player Player;

    public IdleState(Player player)
    {
        Player = player;
    }

    public void Enter()
    {

    }

    public void Update()
    {
        Vector2 input = Player.Input.MoveInput;
        bool isJump = Player.Input.IsJump;
        bool isGround = Player.Move.IsGround();

        if (isJump && isGround)
        {
            Player.StateMachine.ChangeState(new JumpState(Player));
            return;
        }

        if(input.sqrMagnitude > 0.01f)
        {
            Player.StateMachine.ChangeState(new MoveState(Player));
            return;
        }
    }

    public void Exit()
    {

    }
}
