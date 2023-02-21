using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEditor;
using UnityEngine;

namespace Util.UnityMVVM
{
    [CustomEditor(typeof(OneWayPropertyBinding))]
    public class OneWayPropertyBindingEditor : Editor
    {
        private OneWayPropertyBinding m_target;

        private void OnEnable()
        {
            m_target = target as OneWayPropertyBinding;
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();
            
            RenderViewProperty();
            RenderAdapters();
            RenderViewModelProperty();
            
            serializedObject.ApplyModifiedProperties();
        }
        
        private void RenderViewProperty()
        {
            var bindableViewProperties = MVVMHelper.FindBindableViewProperties(m_target.transform);

            int selectedIndex = GetIndexFromPropertyValue(bindableViewProperties, m_target.ViewPropertyName) + 1;

            List<GUIContent> content = new List<GUIContent>() {new GUIContent("None")};
            content.AddRange(GetViewPropertyContentFormatted(bindableViewProperties));
            
            int newSelectedIndex = EditorGUILayout.Popup(new GUIContent("View Property"), selectedIndex, content.ToArray());
            
            if (newSelectedIndex > 0)
            {
                BindableProperty newSelectedProp = bindableViewProperties[newSelectedIndex - 1];

                m_target.ViewPropertyName = newSelectedProp.ToString();

                m_target.m_viewPropertyType = newSelectedProp.Member.PropertyType.ToString();
            }
            else
            {
                m_target.ViewPropertyName = "";
                m_target.ViewPropertyType = "";
            }
        }

        private void RenderAdapters()
        {
            List<string> adapterTypes = new List<string>();
            
            foreach(var adapter in MVVMHelper.FindAdaptersOfType(m_target.ViewPropertyType))
                adapterTypes.Add(adapter.ToString());

            int selectedIndex = adapterTypes.IndexOf(m_target.ViewPropertyAdapter) + 1;

            List<GUIContent> content = new List<GUIContent>() {new GUIContent("None")};
            
            foreach(string adapter in adapterTypes)
                content.Add(new GUIContent(adapter));
            
            int newSelectedIndex = EditorGUILayout.Popup(new GUIContent("View Property Adapter"), selectedIndex, content.ToArray());
            
            if (newSelectedIndex > 0)
            {
                m_target.m_viewPropertyAdapter = adapterTypes[newSelectedIndex - 1];
            }
            else
            {
                m_target.ViewPropertyAdapter = "";
            }
        }
        private void RenderViewModelProperty()
        {
            List<BindableProperty> bindableViewModelProperties = MVVMHelper.FindBindableViewModelProperties(m_target);
            
            int selectedIndex = GetIndexFromPropertyValue(bindableViewModelProperties, m_target.m_viewModelPropertyName) + 1;
            
            List<GUIContent> content = new List<GUIContent>() {new GUIContent("None")};
            content.AddRange(GetViewModelPropertyContentFormatted(bindableViewModelProperties));

            int newSelectedIndex = EditorGUILayout.Popup(new GUIContent("View Model Property"), selectedIndex, content.ToArray());
            
            if (newSelectedIndex > 0)
            {
                BindableProperty newSelectedProp = bindableViewModelProperties[newSelectedIndex - 1];

                m_target.ViewModelPropertyName = newSelectedProp.ToString();
            }
            else
            {
                m_target.ViewModelPropertyName = "";
            }
        }

        private List<GUIContent> GetViewPropertyContentFormatted(List<BindableProperty> properties)
        {
            List<GUIContent> content = new List<GUIContent>();
            
            foreach (var prop in properties)
            {
                GUIContent elementContent = new GUIContent(string.Format("{0} / {1} : {2}", prop.ViewModelTypeName, prop.MemberName, prop.Member.PropertyType.Name));
                
                content.Add(elementContent);
            }

            return content;
        }
        
        private List<GUIContent> GetViewModelPropertyContentFormatted(List<BindableProperty> properties)
        {
            List<GUIContent> content = new List<GUIContent>();
            
            foreach (var prop in properties)
            {
                GUIContent elementContent = new GUIContent(string.Format("{0} / {1} : {2}", prop.ViewModelTypeName, prop.MemberName, prop.Member.PropertyType.Name));

                content.Add(elementContent);
            }

            return content;
        }
        
        private int GetIndexFromPropertyValue(List<BindableProperty> propertyList, string value)
        {
            if (string.IsNullOrEmpty(value))
                return -1;
            
            for (int i = 0; i < propertyList.Count; i++)
            {
                BindableProperty property = propertyList[i];

                if (property.ToString() == value)
                    return i;
            }

            return -1;
        }
    }
}
