using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Level Config", menuName = "Project 100/Data/EnvironmentConfig")]
public class EnvironmentConfig : ScriptableObject
{
    [SerializeField] 
    private GameObject m_environmentPrefab;

    public GameObject EnvironmentPrefab => m_environmentPrefab;
}
