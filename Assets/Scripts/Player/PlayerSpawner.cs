using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSpawner : MonoBehaviour
{
    [SerializeField] 
    private PlayerData m_playerData;

    private void OnEnable()
    {
        RoundManager.Instance.OnRoundStartEvent += OnRoundStart;
    }

    private void OnDisable()
    {
        RoundManager.Instance.OnRoundStartEvent -= OnRoundStart;
    }
    
    private void Spawn()
    {
        if (m_playerData == null)
        {
            Debug.LogError("Cannot spawn player. Player data is null", gameObject);
            return;
        }
        
        if (m_playerData.m_prefab == null)
        {
            Debug.LogError("Cannot spawn player. Prefab in player data is null", gameObject);
            return;
        }

        Instantiate(m_playerData.m_prefab, transform.position, Quaternion.identity);
    }
    
    private void OnRoundStart(RoundData round)
    {
        Spawn();
    }
}
