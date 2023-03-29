using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Util.UnityMVVM;

public class UnitDirectionArrowViewModel : ViewModelMonoBehaviour
{
    public Color m_defaultColor = Color.gray;
    public Color m_selectedColor = Color.green;
    public Vector3 m_direction;
    
    public event Action<UnitDirectionArrowViewModel> OnClickedEvent;

    public Image m_image;

    private void Awake()
    {
        m_image.color = m_defaultColor;
    }

    [Binding]
    public void OnClicked()
    {
        OnClickedEvent?.Invoke(this);
    }

    public void Select(bool isSelected)
    {
        m_image.color = isSelected ? m_selectedColor : m_defaultColor;
    }
}
