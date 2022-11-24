using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ToggleSlider : MonoBehaviour, IPointerUpHandler, IPointerDownHandler
{
    public Slider slider;


    private bool m_Value = false;
    public bool Value
    {
        get=> m_Value;
        set
        {
            if (m_Value = value)
                return;

            m_Value = value;
            ScheduleAnimation(m_Value ? 1.0f : 0f);
            onToggle.Invoke(m_Value);
        }
    }

    public UnityEvent<bool> onToggle;
    


    LTDescr desc=null;

    public void Start()
    {
        if (slider)
        {
            m_Value = Mathf.RoundToInt(slider.value) > Mathf.Abs(slider.maxValue - slider.minValue) / 2.0f;
        }
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if (!slider)
            return;

        // Animate slider to closest position
        m_Value = Mathf.RoundToInt(slider.value) > Mathf.Abs(slider.maxValue - slider.minValue) / 2.0f;
        float animationTargetValue = m_Value ? 1.0f : 0f;

        ScheduleAnimation(animationTargetValue);

        onToggle.Invoke(m_Value);
    }

    public void ScheduleAnimation(float animationTargetValue)
    {
        // Cancel previous animation if exists
        if (desc != null)
            LeanTween.cancel(desc.id);

        desc = gameObject.LeanValue(slider.value, animationTargetValue, .2f).setEaseOutQuart().setDestroyOnComplete(true).setOnComplete(() => { desc = null; }).setOnUpdate((float x) => { slider.value = x; });

    }


    public void OnPointerDown(PointerEventData eventData)
    {
        if(desc!=null)
            LeanTween.cancel(desc.id);
    }

    public void Reset()
    {
        TryGetComponent(out slider);
    }

    
}
