using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(NetworkActionListener))]
public class NetworkActionListenerInspector : Editor
{
    public override void OnInspectorGUI()
    {
        serializedObject.Update();
        
        SerializedProperty onStartHostAttemptEvent = serializedObject.FindProperty(nameof(NetworkActionListener.OnStartHostAttemptEvent));
        SerializedProperty onStartHostEvent = serializedObject.FindProperty(nameof(NetworkActionListener.OnStartHostEvent));
        SerializedProperty onClientConnectionAttemptEvent = serializedObject.FindProperty(nameof(NetworkActionListener.OnClientConnectionAttemptEvent));
        SerializedProperty onClientConnectedEvent = serializedObject.FindProperty(nameof(NetworkActionListener.OnClientConnectedEvent));
        SerializedProperty onClientDisconnectedEvent = serializedObject.FindProperty(nameof(NetworkActionListener.OnClientDisconnectedEvent));
        SerializedProperty singleErrorListener = serializedObject.FindProperty(nameof(NetworkActionListener.m_singleErrorListener));
        SerializedProperty onClientErrorEvent = serializedObject.FindProperty(nameof(NetworkActionListener.OnClientErrorEvent));

        EditorGUILayout.PropertyField(onStartHostAttemptEvent);
        EditorGUILayout.PropertyField(onStartHostEvent);
        EditorGUILayout.PropertyField(onClientConnectionAttemptEvent);
        EditorGUILayout.PropertyField(onClientConnectedEvent);
        EditorGUILayout.PropertyField(onClientDisconnectedEvent);
        EditorGUILayout.PropertyField(singleErrorListener);
        
        if (singleErrorListener.boolValue)
        {
            SerializedProperty errorTypeToListenFor = serializedObject.FindProperty(nameof(NetworkActionListener.m_errorTypeToListenFor));
            EditorGUILayout.PropertyField(errorTypeToListenFor);
        }
        
        EditorGUILayout.PropertyField(onClientErrorEvent);
        
        serializedObject.ApplyModifiedProperties();
    }
}
