using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;

namespace Util.UnityMVVM
{
    public static class PropertyFinder
    {
        /// <summary>
        /// List of types to exclude from the types of components in the UI we can bind to.
        /// </summary>
        private static readonly HashSet<Type> hiddenTypes = new HashSet<Type>{
            typeof(OneWayPropertyBinding),
        };

        /// <summary>
        /// Use reflection to find all components with properties we can bind to.
        /// </summary>
        public static List<BindableProperty> GetBindableProperties(GameObject gameObject) //todo: Maybe move this to the TypeResolver.
        {
            List<BindableProperty> bindableProperties = new List<BindableProperty>();

            Component[] components = gameObject.GetComponents<Component>();

            foreach (Component component in components)
            {
                Type type = component.GetType();

                PropertyInfo[] properties = type.GetProperties(BindingFlags.Instance | BindingFlags.Public);

                foreach (PropertyInfo propertyInfo in properties)
                {
                    if(propertyInfo.GetGetMethod(false) == null || propertyInfo.GetSetMethod(false) == null)
                        continue;
                    
                    if(hiddenTypes.Contains(type))
                        continue;
                    
                    if(propertyInfo.GetCustomAttributes(typeof(ObsoleteAttribute), true).Any())
                        continue;
                    
                    bindableProperties.Add(new BindableProperty(propertyInfo, type));
                }
            }

            return bindableProperties;
        }
    }
}