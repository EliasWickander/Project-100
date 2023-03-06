using System;
using System.Reflection;
using UnityEditor;
using UnityEngine;
using Object = System.Object;

namespace wild
{
    //Variable Group. Container for variables that are used to control a tool's behaviour
    public abstract class ILevelEditorVariableGroup : ScriptableObject
    {
        public event Action OnUpdated;
        
        public void Render(Rect controlsRect)
        {
            Rect fieldRect = new Rect(controlsRect.x, controlsRect.y, controlsRect.width, 20);

            //The fields in the group are rendered below each other
            fieldRect.y += 15f;

            SerializedObject so = new SerializedObject(this);

            foreach (FieldInfo field in GetType().GetFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic))
            {
                SerializedProperty property = so.FindProperty(field.Name);

                EditorGUI.PropertyField(fieldRect, property);

                fieldRect.y += 50;
            }

            if (GUI.Button(fieldRect, "Update"))
            {
                OnUpdated?.Invoke();
            }

            so.ApplyModifiedProperties();
        }
    }
}