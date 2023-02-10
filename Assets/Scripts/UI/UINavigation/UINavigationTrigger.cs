using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UINavigationTrigger : MonoBehaviour
{
    public enum Element
    {
        Screen,
        Panel
    }

    public enum Mode
    {
        NavigateTo,
        NavigateBack,
    }

    public Element m_target;
    public Mode m_mode;
    public UIScreenData m_screenData;
    public UIPanelData m_panelData;

    public void Trigger()
    {
        UINavigation uiNavigation = UINavigation.Instance;
        
        if (m_target == Element.Screen)
        {
            uiNavigation.NavigateTo(m_screenData);
        }
        else if (m_target == Element.Panel)
        {
            if (m_mode == Mode.NavigateTo)
            {
                uiNavigation.NavigateTo(m_panelData);
            }
            else if (m_mode == Mode.NavigateBack)
            {
                uiNavigation.NavigateBack();
            }
        }
    }
}
