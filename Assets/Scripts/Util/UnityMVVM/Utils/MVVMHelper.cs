using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;

namespace Util.UnityMVVM
{
    public class MVVMHelper : MonoBehaviour
    {
        public static List<BindableProperty> FindBindableProperties(OneWayPropertyBinding target)
        {
            List<Type> viewModelTypes = FindAvailableViewModelTypes(target);

            List<BindableProperty> bindableProperties = new List<BindableProperty>();

            foreach (Type viewModel in viewModelTypes)
            {
                IEnumerable<PropertyInfo> publicProperties = GetPublicProperties(viewModel);

                foreach (PropertyInfo property in publicProperties)
                {
                    if (property.GetCustomAttributes(typeof(BindingAttribute), false).Length <= 0)
                        continue;
                    
                    if(property.PropertyType.ToString() != target.ViewPropertyType)
                        continue;
                    
                    bindableProperties.Add(new BindableProperty(property, viewModel));
                }
            }

            return bindableProperties;
        }
        
        private static List<Type> FindAvailableViewModelTypes(OneWayPropertyBinding memberBinding)
        {
            List<Type> viewModelTypes = new List<Type>();

            var transform = memberBinding.transform;

            while (transform != null)
            {
                var components = transform.GetComponents<MonoBehaviour>();
                foreach (var component in components)
                {
                    // Can't bind to self or null
                    if (component == null || component == memberBinding)
                    {
                        continue;
                    }
                    
                    if(component is ViewModelMonoBehaviour)
                        viewModelTypes.Add(component.GetType());
                }
                
                transform = transform.parent;
            }

            return viewModelTypes;
        }

        public static List<Type> FindAdapters(string typeName)
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
        public static ViewModelMonoBehaviour FindViewModel(Transform target, string name)
        {
            Transform transform = target;

            while (transform != null)
            {
                var components = transform.GetComponents<ViewModelMonoBehaviour>();
                foreach (var component in components)
                {
                    if (component == null)
                        continue;

                    if (component.name == name)
                        return component;
                }
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
