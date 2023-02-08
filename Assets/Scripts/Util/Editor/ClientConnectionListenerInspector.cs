using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(ClientConnectionListener))]
public class ClientConnectionListenerInspector : Editor
{
    public override void OnInspectorGUI()
    {
        serializedObject.Update();
        
        SerializedProperty onClientConnectedEvent = serializedObject.FindProperty(nameof(ClientConnectionListener.OnClientConnectedEvent));
        SerializedProperty onClientDisconnectedEvent = serializedObject.FindProperty(nameof(ClientConnectionListener.OnClientDisconnectedEvent));
        SerializedProperty singleErrorListener = serializedObject.FindProperty(nameof(ClientConnectionListener.m_singleErrorListener));
        SerializedProperty onClientErrorEvent = serializedObject.FindProperty(nameof(ClientConnectionListener.OnClientErrorEvent));

        EditorGUILayout.PropertyField(onClientConnectedEvent);
        EditorGUILayout.PropertyField(onClientDisconnectedEvent);
        EditorGUILayout.PropertyField(singleErrorListener);
        
        if (singleErrorListener.boolValue)
        {
            SerializedProperty errorTypeToListenFor = serializedObject.FindProperty(nameof(ClientConnectionListener.m_errorTypeToListenFor));
            EditorGUILayout.PropertyField(errorTypeToListenFor);
        }
        
        EditorGUILayout.PropertyField(onClientErrorEvent);
        
        serializedObject.ApplyModifiedProperties();
    }
}
