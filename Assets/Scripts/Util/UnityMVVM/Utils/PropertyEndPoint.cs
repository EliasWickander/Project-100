using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

namespace Util.UnityMVVM
{
    public class PropertyEndPoint
    {
        private Component m_propertyOwner;
        private PropertyInfo m_property;
        private PropertyAdapter m_adapter;
        
        public PropertyEndPoint(Component propertyOwner, string propertyName, PropertyAdapter adapter)
        {
            if (propertyOwner == null)
            {
                Debug.LogError($"Attempted to create property end point with null propertyOwner");
                return;
            }
            
            m_propertyOwner = propertyOwner;
            m_adapter = adapter;
            m_property = propertyOwner.GetType().GetProperty(propertyName);
            
            if (m_property == null)
                Debug.LogError($"Attempted to create property end point for property '{propertyName}' of object {propertyOwner} but no such property exists");
        }

        public void SetValue(object value)
        {
            if(m_property == null)
                return;

            if (m_adapter != null)
                value = m_adapter.Convert(value);
            
            m_property.SetValue(m_propertyOwner, value);
        }

        public object GetValue()
        {
            if (m_property == null)
                return null;

            return m_property.GetValue(m_propertyOwner);
        }
    }
}
