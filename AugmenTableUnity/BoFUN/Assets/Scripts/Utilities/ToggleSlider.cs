using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;


namespace BoFUN.UI
{
    /// <summary>
    /// Used with Unity.UI.Slider to make it have a toggle behaviour
    /// </summary>
    public class ToggleSlider : MonoBehaviour, IPointerUpHandler, IPointerDownHandler
    {
        /// <summary>
        /// The slider to convert to toggle
        /// </summary>
        public Slider slider;
        private bool m_Value = false;
        /// <summary>
        /// The current value of the slider
        /// </summary>
        public bool Value
        {
            get => m_Value;
            set
            {
                if (m_Value = value)
                    return;

                m_Value = value;
                if (slider)
                {
                    slider.wholeNumbers = false;
                    ScheduleAnimation(m_Value ? 1.0f : 0f);
                }
                onToggle.Invoke(m_Value);
            }
        }
        public UnityEvent<bool> onToggle;


        LTDescr desc = null;

        public void Start()
        {
            if (slider)
            {
                slider.minValue = 0f;
                slider.maxValue = 1.0f;
                slider.wholeNumbers = true;
                m_Value = slider.normalizedValue >= .5f;
            }
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            if (!slider)
                return;

            // Animate slider to closest position
            m_Value = slider.normalizedValue >= .5f;
            float animationTargetValue = m_Value ? 1.0f : 0f;

            ScheduleAnimation(animationTargetValue);

            onToggle.Invoke(m_Value);
        }

        public void ScheduleAnimation(float animationTargetValue)
        {
            // Cancel previous animation if exists
            if (desc != null)
                LeanTween.cancel(desc.id);

            desc = gameObject.LeanValue(slider.value, animationTargetValue, .2f).setEaseOutQuart().setOnComplete(() => { desc = null; slider.wholeNumbers = true; }).setOnUpdate((float x) => { slider.value = x; });

        }


        public void OnPointerDown(PointerEventData eventData)
        {
            if (desc != null)
                LeanTween.cancel(desc.id);

            if (slider)
            {
                slider.wholeNumbers = false;
            }
        }

        public void Reset()
        {
            TryGetComponent(out slider);
        }


        public void OnDestroy()
        {
            if (desc != null)
            {
                LeanTween.cancel(desc.id);
            }
        }


    }
}