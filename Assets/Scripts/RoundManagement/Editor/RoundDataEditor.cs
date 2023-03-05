using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(RoundData))]
public class RoundDataEditor : Editor
{
    private RoundData m_roundData;

    private void OnEnable()
    {
        m_roundData = (RoundData) target;
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        SerializedProperty roundName = serializedObject.FindProperty(nameof(RoundData.m_roundName));
        SerializedProperty waves = serializedObject.FindProperty(nameof(RoundData.m_waves));

        EditorGUILayout.PropertyField(roundName);
        EditorGUILayout.PropertyField(waves);

        serializedObject.ApplyModifiedProperties();
    }
}
