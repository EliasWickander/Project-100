using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomPropertyDrawer(typeof(RoundDataAttribute))]
public class RoundDataAttributePropertyDrawer : PropertyDrawer
{
    private WorldData[] m_worlds;

    private RoundData m_selectedRound;

    private bool m_changedSelection = false;

    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        if(property == null)
            return;

        if (m_worlds == null)
            m_worlds = Resources.FindObjectsOfTypeAll<WorldData>();

        UpdatePropertySelection(property);

        m_selectedRound = property.objectReferenceValue != null ? property.objectReferenceValue as RoundData : null;
        
        position = EditorGUI.PrefixLabel(position, new GUIContent("Round"));
        bool activateDropdown = EditorGUI.DropdownButton(position, m_selectedRound != null ? new GUIContent($"{m_selectedRound.name}") : new GUIContent("Nothing"), FocusType.Passive, EditorStyles.popup);

        if (activateDropdown)
        {
            ShowDropdown(position, m_selectedRound);
        }
    }

    private void UpdatePropertySelection(SerializedProperty property)
    {
        if (m_changedSelection)
        {
            property.objectReferenceValue = m_selectedRound;
            if (property.serializedObject != null)
                property.serializedObject.ApplyModifiedProperties();
            AssetDatabase.SaveAssets();

            m_changedSelection = false;
        }
    }
    private void ShowDropdown(Rect position, ScriptableObject selectedObject)
    {
        GenericMenu menu = new GenericMenu();
        menu.allowDuplicateNames = true;
        menu.AddItem(new GUIContent("Nothing"), selectedObject == null, OnRoundSelected, null);
        menu.AddSeparator("");

        if (m_worlds != null)
        {
            foreach (WorldData world in m_worlds)
            {
                if(world == null)
                    continue;

                foreach (RoundData round in world.m_rounds)
                {
                    GUIContent roundEntry = new GUIContent($"{world.name}/{round.name}");
                    menu.AddItem(roundEntry, round == selectedObject, OnRoundSelected, round);
                }
            }   
        }
        
        menu.DropDown(position);
    }

    private void OnRoundSelected(object round)
    {
        m_selectedRound = round as RoundData;

        m_changedSelection = true;
    }
}
