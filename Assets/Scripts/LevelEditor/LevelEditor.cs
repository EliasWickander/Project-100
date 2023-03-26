using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Util.AdvancedTypes;

public class LevelEditor : MonoBehaviour
{
    public LevelEditorGridViewModel m_grid;

    private void Awake()
    {
        if (m_grid == null)
            m_grid = FindObjectOfType<LevelEditorGridViewModel>();
    }

    private void Update()
    {
        //TODO: Do timeline (slider) based approach for wave management
        if (Input.GetMouseButtonDown(0))
        {

        } 
    }
}
