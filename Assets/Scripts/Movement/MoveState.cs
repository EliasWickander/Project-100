using UnityEngine;
using Util.StateMachine;

public abstract class MoveState : IState
{
    protected MoveStateMachine m_moveStateMachine;
    protected MoveInfo m_moveInfo;
    protected Rigidbody m_rigidbody;

    public MoveState(MoveStateMachine stateMachine, MoveInfo moveInfo, Rigidbody rigidbody) 
    {
        m_moveStateMachine = stateMachine;
        m_moveInfo = moveInfo;
        m_rigidbody = rigidbody;
    }
    
    public virtual void OnEnter(IState prevState) { }

    public virtual void Update() { }

    public virtual void OnExit(IState nextState) { }
}
