using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSpawner : CharacterSpawner<PlayerData>
{
    private void OnEnable()
    {
        RoundManager.Instance.OnRoundStartEvent += OnRoundStart;
    }

    private void OnDisable()
    {
        RoundManager.Instance.OnRoundStartEvent -= OnRoundStart;
    }

    private void OnRoundStart(RoundData round)
    {
        Spawn();
    }
}
