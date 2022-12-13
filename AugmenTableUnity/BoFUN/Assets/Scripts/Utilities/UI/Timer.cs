using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Timer : MonoBehaviour
{
    public Image timeRemainingGraphic;
    public TMP_Text timeRemainingText;

    public Color fullColor;
    public Color emptyColor;

    private float m_TotalTimeInSeconds = 60;
    private float m_RemainingTimeInSeconds = 60;
    public float TotalTimeInSeconds
    {
        get => m_TotalTimeInSeconds;
        set
        {
            m_TotalTimeInSeconds = Mathf.Max(value, 0);
            m_RemainingTimeInSeconds = Mathf.Min(m_RemainingTimeInSeconds, m_TotalTimeInSeconds);
        }
    }
    public float RemainingTimeInSeconds
    {
        get => m_RemainingTimeInSeconds;
        set
        {
            m_RemainingTimeInSeconds = Mathf.Min(value, m_TotalTimeInSeconds);

            // Update graphic
            if (timeRemainingGraphic)
            {
                timeRemainingGraphic.fillAmount = m_RemainingTimeInSeconds / m_TotalTimeInSeconds;
            }
            if (timeRemainingText)
            {
                TimeSpan ts = TimeSpan.FromSeconds(m_RemainingTimeInSeconds);

                // Set Color
                if(m_RemainingTimeInSeconds > m_TotalTimeInSeconds / 2.0f)
                {
                    timeRemainingText.color = fullColor;
                }
                else
                {
                    timeRemainingText.color = emptyColor;
                }

                timeRemainingText.text = $"{ts.Minutes:0}:{ts.Seconds:00}";
            }
        }
    }


    void Start()
    {
        if(!timeRemainingGraphic || timeRemainingGraphic.type != Image.Type.Filled)
        {
            Debug.LogError($"{gameObject}<Timer>: Incorrect setup");
        }
    }

}
