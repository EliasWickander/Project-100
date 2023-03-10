using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Enemy Data", menuName = "Project 100/Data/EnemyData")]
public class EnemyData : CharacterData
{
    [Header("Info")]
    public string m_displayName = "Enemy";
    public Texture2D m_icon;
}
