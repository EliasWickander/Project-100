using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

[CreateAssetMenu(fileName = "New World Data", menuName = "Project 100/Data/WorldData")]
public class WorldData : ScriptableObject
{
    [FormerlySerializedAs("m_roundData")] public List<RoundData> m_rounds;
}
