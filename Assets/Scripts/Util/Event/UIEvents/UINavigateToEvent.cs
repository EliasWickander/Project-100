using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class UINavigateToEventHolder
{
    public enum Target
    {
        Screen,
        Panel
    }

    public Target m_target;
}

[CreateAssetMenu(fileName = "New UINavigateToEvent", menuName = "Project 100/GameEvents/UINavigateToEvent")]
public class UINavigateToEvent : GameEvent<UINavigateToEventHolder>
{

}
