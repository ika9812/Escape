using UnityEngine;

public interface IPlayerState
{
    void Enter();
    void Exit();
    void Update();
}   

public class PlayerStateMachine 
{
    public IPlayerState CurrentState { get; private set; }

    public void ChangeState(IPlayerState newState)
    {
        CurrentState?.Exit();

        CurrentState = newState;

        CurrentState?.Enter();
    }

    public void Update()
    {
        CurrentState?.Update();
    }
}
