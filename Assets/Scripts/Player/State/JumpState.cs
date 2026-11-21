using UnityEngine;

public class JumpState : IPlayerState
{
    Player Player;
    
    public JumpState(Player player)
    {
        Player = player;
    }

    public void Enter()
    {
        Player.Move.Jump();
    }

    public void Update()
    {
        Vector2 input = Player.Input.MoveInput;
        bool isJump = Player.Input.IsJump;
        bool isGround = Player.Move.IsGround();

        Player.Move.Move(Player.transform);
        if(isGround)
        {
            if(input.sqrMagnitude > 0.01f)
            {
                Player.StateMachine.ChangeState(new MoveState(Player));
            }
            else
            {
                Player.StateMachine.ChangeState(new IdleState(Player));
            }
            return;
        }
    }

    public void Exit()
    {

    }
}
