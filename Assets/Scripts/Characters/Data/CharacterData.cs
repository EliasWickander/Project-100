using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Util.UnityMVVM;

[Binding]
public abstract class CharacterData : ViewModelScriptableObject
{
    public GameObject m_prefab;

    [Header("Movement")]
    public float m_acceleration;
    public float m_maxSpeed;
}
