using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UINavigateToTrigger : MonoBehaviour
{
    public enum Element
    {
        Screen,
        Panel
    }
    
    public Element m_target;
    public UIScreenData m_screenData;
    public UIPanelData m_panelData;
    
    public void Trigger()
    {
        UINavigation uiNavigation = UINavigation.Instance;
        
        if (m_target == Element.Screen)
        {
            if(m_screenData)
                uiNavigation.NavigateTo(m_screenData);
        }
        else if (m_target == Element.Panel)
        {
            if(m_panelData)
                uiNavigation.NavigateTo(m_panelData);
        }
    }
}
