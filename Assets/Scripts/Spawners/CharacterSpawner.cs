using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class CharacterSpawner<T> : MonoBehaviour where T : CharacterData
{
    [SerializeField] 
    private T m_data;

    public virtual void Spawn()
    {
        if (m_data == null)
        {
            Debug.LogError($"Cannot spawn character of type {typeof(T)}. Character data is null", gameObject);
            return;
        }
        
        if (m_data.m_prefab == null)
        {
            Debug.LogError($"Cannot spawn character of type {typeof(T)}. Prefab in character data is null", gameObject);
            return;
        }

        GameObject instance = Instantiate(m_data.m_prefab, transform.position, Quaternion.identity);
        
        Character spawnedCharacter = instance.GetComponent<Character>();

        if (spawnedCharacter == null)
        {
            Debug.LogError("Spawned character had no component deriving from Character attached. Click to locate", instance);
            return;
        }
        
        spawnedCharacter.OnSpawn(m_data);
    }
}
