using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour
{
    private CharacterData m_data;
    public CharacterData Data => m_data;
    
    private MoveInfo m_moveInfo = null;
    public MoveInfo MoveInfo => m_moveInfo;
    
    public virtual void OnSpawn(CharacterData data)
    {
        m_data = data;
        
        ResetMoveStateInfo();
    }

    private void ResetMoveStateInfo()
    {
        m_moveInfo = new MoveInfo();

        m_moveInfo.Acceleration = m_data.m_acceleration;
        m_moveInfo.MaxSpeed = m_data.m_maxSpeed;
    }
}
