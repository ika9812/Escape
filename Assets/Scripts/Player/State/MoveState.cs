using UnityEngine;

public class MoveState : IPlayerState
{
    Player Player;

    public MoveState(Player player)
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

        Player.Move.Move(Player.transform);

        if(isJump && isGround)
        {
            Player.StateMachine.ChangeState(new JumpState(Player));
            return;
        }
        
        if(input.sqrMagnitude< 0.01f)
        {
            Player.StateMachine.ChangeState(new IdleState(Player));
            return;
        }
    }

    public void Exit()
    {

    }
}
