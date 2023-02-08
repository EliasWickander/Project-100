using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIScreen : MonoBehaviour
{
    public UIScreenData m_screenData;
    private Dictionary<UIPanelData, UIPanel> m_panels = new Dictionary<UIPanelData, UIPanel>();
    
    public Stack<UIPanel> ActivePanels => m_activePanels;
    private Stack<UIPanel> m_activePanels = new Stack<UIPanel>();
    
    public void Setup()
    {
        foreach (UIPanelData panel in m_screenData.m_panels)
        {
            if(panel.m_uiPanelPrefab == null)
                continue;

            UIPanel panelInstance = Instantiate(panel.m_uiPanelPrefab, transform);
            panelInstance.m_panelData = panel;
            panelInstance.Setup();
            
            panelInstance.SetVisible(false);
            
            m_panels.Add(panel, panelInstance);

            if (panel.m_activeOnStart)
            {
                NavigateTo(panel);
            }
        }
    }

    public void Reset()
    {
        for (int i = 0; i < m_activePanels.Count; i++)
        {
            UIPanel topPanel = m_activePanels.Peek();

            if (topPanel.m_panelData.m_activeOnStart)
                break;
            
            topPanel.SetVisible(false);
            m_activePanels.Pop();
        }
    }
    public void SetVisible(bool visible)
    {
        if (visible == false)
        {
            Reset();
        }
        
        gameObject.SetActive(visible);
    }

    public void NavigateTo(UIPanelData panel)
    {
        UIPanel panelObject = m_panels[panel];

        if(m_activePanels.Contains(panelObject))
            return;

        m_activePanels.Push(panelObject);
        panelObject.SetVisible(true);
    }

    public void NavigateBack()
    {
        if(m_activePanels.Count <= 0)
            return;

        UIPanel topPanel = m_activePanels.Pop();
        topPanel.SetVisible(false);
    }
}
