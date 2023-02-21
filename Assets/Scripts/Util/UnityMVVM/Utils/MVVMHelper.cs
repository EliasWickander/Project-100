using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;

namespace Util.UnityMVVM
{
    public class MVVMHelper
    {
        /// <summary>
        /// Find all bindable properties on view models known to target binding
        /// </summary>
        /// <param name="target">Target binding</param>
        /// <returns></returns>
        public static List<BindableProperty> FindBindableViewModelProperties(OneWayPropertyBinding target)
        {
            List<ViewModelMonoBehaviour> tentativeViewModels = FindViewModelsInHierarchy(target.transform);

            List<BindableProperty> bindableProperties = new List<BindableProperty>();

            foreach (ViewModelMonoBehaviour viewModel in tentativeViewModels)
            {
                Type viewModelType = viewModel.GetType();
                
                IEnumerable<PropertyInfo> publicProperties = GetPublicProperties(viewModelType);

                foreach (PropertyInfo property in publicProperties)
                {
                    if (property.GetCustomAttributes(typeof(BindingAttribute), false).Length <= 0)
                        continue;
                    
                    if(property.PropertyType.ToString() != target.ViewPropertyType)
                        continue;
                    
                    bindableProperties.Add(new BindableProperty(property, viewModelType));
                }
            }

            return bindableProperties;
        }
        
        /// <summary>
        /// Find all bindable properties on components attached to target
        /// </summary>
        /// <param name="target">Target transform</param>
        /// <returns></returns>
        public static List<BindableProperty> FindBindableViewProperties(Transform target)
        {
            List<BindableProperty> bindableProperties = new List<BindableProperty>();

            Component[] components = target.GetComponents<Component>();

            foreach (Component component in components)
            {
                Type type = component.GetType();
                
                if(type.IsSubclassOf(typeof(PropertyBinding)))
                    continue;
                
                IEnumerable<PropertyInfo> properties = GetPublicProperties(type);

                foreach (PropertyInfo propertyInfo in properties)
                {
                    if(propertyInfo.GetGetMethod(false) == null || propertyInfo.GetSetMethod(false) == null)
                        continue;

                    if(propertyInfo.GetCustomAttributes(typeof(ObsoleteAttribute), true).Length > 0)
                        continue;
                    
                    bindableProperties.Add(new BindableProperty(propertyInfo, type));
                }
            }

            return bindableProperties;
        }
        
        /// <summary>
        /// Find all view models upwards in hierarchy from source transform
        /// </summary>
        /// <param name="source">Source transform to start the search from</param>
        /// <returns></returns>
        private static List<ViewModelMonoBehaviour> FindViewModelsInHierarchy(Transform source)
        {
            List<ViewModelMonoBehaviour> foundViewModels = new List<ViewModelMonoBehaviour>();

            var transform = source;

            while (transform != null)
            {
                foreach (ViewModelMonoBehaviour viewModel in transform.GetComponents<ViewModelMonoBehaviour>())
                {
                    if(viewModel == null)
                        continue;
                    
                    foundViewModels.Add(viewModel);
                }
                
                transform = transform.parent;
            }

            return foundViewModels;
        }
        
        /// <summary>
        /// Finds view model upwards in hierarchy from source transform
        /// </summary>
        /// <param name="source">Source transform to start the search from</param>
        /// <param name="viewModelName">Name of view model to look for</param>
        /// <returns></returns>
        public static ViewModelMonoBehaviour FindViewModelInHierarchy(Transform source, string viewModelName)
        {
            Transform transform = source;

            while (transform != null)
            {
                Component foundComponent = transform.GetComponent(viewModelName);

                if (foundComponent)
                {
                    if (foundComponent is ViewModelMonoBehaviour foundViewModel)
                        return foundViewModel;
                    
                    Debug.LogError($"Found component with name {viewModelName} but it is not derived from ViewModelMonoBehaviour. Something is wrong");
                    return null;
                }
                
                transform = transform.parent;
            }

            return null;
        }

        /// <summary>
        /// Finds all property adapters of type in executing assembly
        /// </summary>
        /// <param name="typeName">Name of type (including namespaces)</param>
        /// <returns></returns>
        public static List<Type> FindAdaptersOfType(string typeName)
        {
            List<Type> result = new List<Type>();

            Assembly assembly = Assembly.GetExecutingAssembly();

            foreach (Type type in assembly.GetTypes())
            {
                if(type.BaseType == null)
                    continue;
                
                if(!type.BaseType.IsGenericType)
                    continue;
                
                if(type.BaseType.GetGenericTypeDefinition() != typeof(PropertyAdapter<>))
                    continue;
 
                if(type.BaseType.GetGenericArguments()[0].ToString() != typeName)
                    continue;
                
                result.Add(type);
            }

            return result;
        }
        
        public static Type FindAdapter(string adapterName)
        {
            Assembly assembly = Assembly.GetExecutingAssembly();

            foreach (Type type in assembly.GetTypes())
            {
                if(type.BaseType == null)
                    continue;
                
                if(!type.BaseType.IsGenericType)
                    continue;
                
                if(type.BaseType.GetGenericTypeDefinition() != typeof(PropertyAdapter<>))
                    continue;

                if(type.ToString() != adapterName)
                    continue;

                return type;
            }

            return null;
        }

        private static IEnumerable<PropertyInfo> GetPublicProperties(Type type)
        {
            if (!type.IsInterface)
            {
                return type.GetProperties(BindingFlags.Public | BindingFlags.Instance);
            }

            return (new[] { type })
                .Concat(type.GetInterfaces())
                .SelectMany(i => i.GetProperties(BindingFlags.Public | BindingFlags.Instance));
        }
    }
}
