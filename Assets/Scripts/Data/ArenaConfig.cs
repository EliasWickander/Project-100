using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Arena Config", menuName = "Project 100/Data/ArenaConfig")]
public class ArenaConfig : ScriptableObject
{
    [SerializeField] 
    private int m_tilesPerRow = 10;

    public int TilesPerRow => m_tilesPerRow;
}
