using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CreateAssetMenu(fileName = "New Level Config", menuName = "Project 100/Data/EnvironmentConfig")]
public class EnvironmentConfig : ScriptableObject
{
    [SerializeField] 
    private GameObject m_environmentPrefab;

    public GameObject EnvironmentPrefab => m_environmentPrefab;

    public string m_resourcePath = "";

    private void OnValidate()
    { 
        m_resourcePath = AssetDatabase.GetAssetPath(this);
        m_resourcePath = m_resourcePath.Replace("Assets/Resources/", "");
        m_resourcePath = System.IO.Path.ChangeExtension(m_resourcePath, null);
    }
}
