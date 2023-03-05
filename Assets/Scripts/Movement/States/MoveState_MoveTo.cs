using UnityEngine;
using Util.StateMachine;
public class MoveState_MoveTo : MoveState
{
    public Vector3 TargetPos { get; set; }
    public MoveState_MoveTo(MoveStateMachine stateMachine, MoveInfo moveInfo, Rigidbody rigidbody) : base(stateMachine, moveInfo, rigidbody)
    {
    }

    public override void Update()
    {
        base.Update();
        
        Vector3 dirToTarget = TargetPos - m_rigidbody.position;
        dirToTarget.y = 0;

        float bias = 0.2f;

        if (dirToTarget.sqrMagnitude > bias)
        {
            dirToTarget.Normalize();

            m_rigidbody.velocity = dirToTarget * m_moveInfo.MaxSpeed;
        }
        else
        {
            m_moveStateMachine.SetState(m_moveStateMachine.IdleState);
        }
    }

    public override void OnExit(IState nextState)
    {
        base.OnExit(nextState);

        m_rigidbody.velocity = Vector3.zero;
    }
}
