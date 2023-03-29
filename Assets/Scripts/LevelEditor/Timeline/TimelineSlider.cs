using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TimelineSlider : Slider
{
    public void SetValue(float newValue)
    {
        value = newValue;
    }
}
