using UnityEngine;
using Util.StateMachine;
public class MoveStateMachine : StateMachine
{
    public MoveState_Idle IdleState { get; private set; }
    public MoveState_MoveTo MoveToState { get; private set; }
    
    public MoveStateMachine(MoveInfo moveInfo, Rigidbody rigidbody)
    {
        IdleState = new MoveState_Idle(this, moveInfo, rigidbody);
        MoveToState = new MoveState_MoveTo(this, moveInfo, rigidbody);
    }
    
    public void SetMoveTo(Vector3 targetPos)
    {
        MoveToState.TargetPos = targetPos;
        
        SetState(MoveToState);
    }
}
