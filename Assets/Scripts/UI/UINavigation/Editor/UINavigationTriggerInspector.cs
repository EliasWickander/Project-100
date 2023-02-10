using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(UINavigationTrigger))]
public class UINavigationTriggerInspector : Editor
{
    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        SerializedProperty target = serializedObject.FindProperty(nameof(UINavigationTrigger.m_target));

        EditorGUILayout.PropertyField(target);


        switch ((UINavigationTrigger.Element) target.intValue)
        {
            case UINavigationTrigger.Element.Screen:
            {
                SerializedProperty screenData = serializedObject.FindProperty(nameof(UINavigationTrigger.m_screenData));

                EditorGUILayout.PropertyField(screenData);
                break;
            }
            case UINavigationTrigger.Element.Panel:
            {
                SerializedProperty mode = serializedObject.FindProperty(nameof(UINavigationTrigger.m_mode));
                EditorGUILayout.PropertyField(mode);

                if ((UINavigationTrigger.Mode) mode.intValue == UINavigationTrigger.Mode.NavigateTo)
                {
                    SerializedProperty panelData = serializedObject.FindProperty(nameof(UINavigationTrigger.m_panelData));
                
                    EditorGUILayout.PropertyField(panelData);
                }
                break;
            }
        }
        serializedObject.ApplyModifiedProperties();
    }
}
