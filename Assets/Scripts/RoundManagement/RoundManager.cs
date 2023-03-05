using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Util.AdvancedTypes;

public class RoundManager : Singleton<RoundManager>
{
    public event Action<RoundData> OnRoundStartEvent;

    public List<RoundData> m_rounds = new List<RoundData>();

    private int m_activeRoundIndex = -1;

    private void Start()
    {
        StartNextRound();
    }

    private void StartNextRound()
    {
        if (m_rounds.Count <= m_activeRoundIndex + 1)
        {
            Debug.LogError("Cannot start next round. No more rounds");
            return;
        }
        
        m_activeRoundIndex++;
        OnRoundStartEvent?.Invoke(m_rounds[m_activeRoundIndex]);
        
        Debug.Log($"Starting round {m_rounds[m_activeRoundIndex].m_roundName}");
    }
}
