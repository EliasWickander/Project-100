using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class LevelEditorContext
{
    public static LevelData s_currentEditedLevel;

    public static void Clear()
    {
        s_currentEditedLevel = null;
    }
}
