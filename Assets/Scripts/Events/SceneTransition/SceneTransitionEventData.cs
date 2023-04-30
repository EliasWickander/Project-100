using System;
using Util.Attributes;

[Serializable]
public class SceneTransitionEventData
{
    [Scene]
    public string m_targetScene;
}