using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;
using UnityEngine.EventSystems;
using Util.UnityMVVM;

[Binding]
public class SelectableUnitViewModel : ViewModelMonoBehaviour, IPointerClickHandler
{
    private PropertyChangedEventArgs m_idProp = new PropertyChangedEventArgs(nameof(Id));
    private string m_id;

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

    [SerializeField] 
    private UnitSelectedGameEvent m_unitSelectedEvent;

    public void OnSelection()
    {
        if(m_unitSelectedEvent != null)
            m_unitSelectedEvent.Raise(this);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        OnSelection();
    }
}
