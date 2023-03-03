using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Util.AdvancedTypes
{
    public class Singleton<T> : MonoBehaviour where T : MonoBehaviour
    {
        public bool m_persistentBetweenScenes;
        private static readonly object m_lock = new object();
        
        private static T s_instance;
        public static T Instance
        {
            get
            {
                lock (m_lock)
                {
                    if (s_instance != null)
                        return s_instance;
    
                    T[] instances = FindObjectsOfType<T>();
    
                    if (instances.Length > 0)
                    {
                        if (instances.Length > 1)
                        {
                            Debug.LogError($"Found {instances.Length} duplicates of Singleton<{typeof(T)}>. Cleaning all but one");
    
                            for (int i = 1; i < instances.Length; i++)
                            {
                                Destroy(instances[i]);
                            }
                        }
    
                        s_instance = instances[0];
                        return s_instance;
                    }
                    else
                    {
                        Debug.LogError($"Failed to find instance of Singleton<{typeof(T)}> in scene. Creating one...");
                        
                        GameObject newObject = new GameObject($"Singleton<{typeof(T)}>");
    
                        s_instance = newObject.AddComponent<T>();
    
                        return s_instance;
                    }
                }
            }
        }
    
        private void Awake()
        {
            if(m_persistentBetweenScenes)
                DontDestroyOnLoad(gameObject);
            
            OnAwake();
        }
    
        protected virtual void OnAwake()
        {
            
        }
    }
}
