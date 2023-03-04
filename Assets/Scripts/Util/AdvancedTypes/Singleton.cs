using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Util.AdvancedTypes
{
    public class Singleton<T> : MonoBehaviour where T : MonoBehaviour
    {
        public bool m_persistentBetweenScenes;

        private static object s_instance = null;
        public static T Instance
        {
            get
            {
                if (s_instance == null)
                {
                    s_instance = FindObjectOfType(typeof(T)) as T;

                    if (s_instance == null)
                    {
                        GameObject newObject = new GameObject($"Singleton<{typeof(T)}>");
                        
                        s_instance = newObject.AddComponent<T>();
                        
                        return (T)s_instance;
                    }
                }

                return (T)s_instance;
            }
        }

        protected virtual void Awake()
        {
            if (s_instance != null && s_instance != this)
            {
                Debug.LogWarning($"Destroying duplicate instance of Singleton<{typeof(T)}>");
                Destroy(Instance.gameObject);
            }
            else
            {
                s_instance = this;
                
                if(m_persistentBetweenScenes)
                    DontDestroyOnLoad(gameObject);   
            }
        }

        protected virtual void OnDestroy()
        {
            if (!m_persistentBetweenScenes)
            {
                if(s_instance == this)
                    s_instance = null;   
            }
        }
    }
}
