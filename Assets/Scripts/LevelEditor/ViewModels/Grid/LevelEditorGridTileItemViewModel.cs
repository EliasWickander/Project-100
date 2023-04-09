using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;
using Util.UnityMVVM;

[Binding]
public class LevelEditorGridTileItemViewModel : ViewModelMonoBehaviour
{
    private readonly PropertyChangedEventArgs m_unitProp = new PropertyChangedEventArgs(nameof(Unit));
    private UnitData m_unit = null;

    [Binding]
    public UnitData Unit
    {
        get
        {
            return m_unit;
        }
        set
        {
            m_unit = value;
            OnPropertyChanged(m_unitProp);
            
            UpdateVariables();
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
    
    public void UpdateVariables()
    {
        Icon = Unit != null ? Unit.Icon : null;
    }
}
