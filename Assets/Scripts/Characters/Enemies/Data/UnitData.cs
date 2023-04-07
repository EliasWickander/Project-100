using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;
using Util.UnityMVVM;

[Binding]
[CreateAssetMenu(fileName = "New Unit Data", menuName = "Project 100/Data/UnitData")]
public class UnitData : CharacterData
{
    [Header("Info")] 
    private PropertyChangedEventArgs m_idProp = new PropertyChangedEventArgs(nameof(Id));
    public string m_id = "";

    [Binding]
    public string Id
    {
        get
        {
            return m_id;
        }
        set
        {
            m_id = value;
            OnPropertyChanged(m_idProp);
        }
    }

    private PropertyChangedEventArgs m_displayNameProp = new PropertyChangedEventArgs(nameof(DisplayName));
    public string m_displayName = "Enemy";

    [Binding]
    public string DisplayName
    {
        get
        {
            return m_displayName;
        }
        set
        {
            m_displayName = value;
            OnPropertyChanged(m_displayNameProp);
        }
    }

    private PropertyChangedEventArgs m_iconProp = new PropertyChangedEventArgs(nameof(Icon));
    public Sprite m_icon;

    [Binding]
    public Sprite Icon
    {
        get
        {
            return m_icon;
        }
        set
        {
            m_icon = value;
            OnPropertyChanged(m_iconProp);
        }
    }
}
