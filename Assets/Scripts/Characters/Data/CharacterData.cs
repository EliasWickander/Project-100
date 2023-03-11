using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class CharacterData : ScriptableObject
{
    public GameObject m_prefab;

    [Header("Movement")]
    public float m_acceleration;
    public float m_maxSpeed;
}
