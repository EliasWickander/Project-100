using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour
{
    public CharacterData m_data;
    
    private MoveStateInfo m_moveStateInfo = null;
    public MoveStateInfo MoveStateInfo => m_moveStateInfo;
    
    public virtual void OnSpawn(CharacterData data)
    {
        m_data = data;
        
        ResetMoveStateInfo();
    }

    private void ResetMoveStateInfo()
    {
        m_moveStateInfo = new MoveStateInfo();

        m_moveStateInfo.Acceleration = m_data.m_acceleration;
        m_moveStateInfo.MaxSpeed = m_data.m_maxSpeed;
    }
}
