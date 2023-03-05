using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Player : Character
{
    private Rigidbody m_rigidbody = null;

    private MoveStateMachine m_moveStateMachine;
    
    private void Awake()
    {
        m_rigidbody = GetComponent<Rigidbody>();
    }

    public override void OnSpawn(CharacterData data)
    {
        base.OnSpawn(data);
        
        m_moveStateMachine = new MoveStateMachine(MoveInfo, m_rigidbody);
        
        m_moveStateMachine.SetState(m_moveStateMachine.IdleState);
    }

    private void Update()
    {
        if (m_moveStateMachine != null)
        {
            m_moveStateMachine.Update();   
            
            HandleMovement();
        }
    }

    private void HandleMovement()
    {
        if (Input.GetMouseButtonDown(1))
        {
            Ray mouseRay = CameraManager.Instance.CurrentCamera.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(mouseRay, out RaycastHit hit, Mathf.Infinity))
            {
                m_moveStateMachine.SetMoveTo(hit.point);   
            }
        }
    }
}
