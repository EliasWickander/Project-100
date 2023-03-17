using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;
using Util.UnityMVVM;

[Binding]
public class SelectableUnitViewModel : ViewModel
{
    private PropertyChangedEventArgs m_displayNameProp = new PropertyChangedEventArgs(nameof(DisplayName));
    private string m_displayName = "Enemy";

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
    private Sprite m_icon;

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
