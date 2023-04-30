using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class TimelineSlider : CustomSlider
{
    private Vector2 m_handleRectStartPos;

    protected override void Awake()
    {
        base.Awake();

        m_handleRectStartPos = HandleRect.position;
    }

    public Vector2 GetHandlePositionFromTimestamp(float timeStamp)
    {
        float percentage = Mathf.Clamp(timeStamp, MinValue, MaxValue);

        float range = MaxValue - MinValue;
        float handlePositionX = m_handleRectStartPos.x + FillContainerRect.rect.width * percentage / range;
        float handlePositionY = HandleRect.anchoredPosition.y;
        
        return new Vector2(handlePositionX, handlePositionY);
    }
}
