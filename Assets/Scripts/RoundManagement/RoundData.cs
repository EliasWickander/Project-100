using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class WaveData
{
    public float m_delay = 1;
}

[CreateAssetMenu(fileName = "New Round Data", menuName = "Project 100/Data/RoundData")]
public class RoundData : ScriptableObject
{
    public string m_roundName;

    public List<WaveData> m_waves = new List<WaveData>();
}
