using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Util.UnityMVVM;

[Binding]
public class LevelEditorGridTileViewModel : LevelEditorTileViewModel
{
    public Vector2Int GridPos { get; set; }
}
