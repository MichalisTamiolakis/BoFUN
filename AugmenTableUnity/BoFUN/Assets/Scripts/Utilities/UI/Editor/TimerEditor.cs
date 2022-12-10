using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(Timer))]
public class TimerEditor : Editor
{
    Timer m_Timer;
    public void OnEnable()
    {
        m_Timer = (Timer)target;
    }

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        EditorGUI.BeginChangeCheck();
        var totalSeconds = EditorGUILayout.FloatField("Total Time In Seconds", m_Timer.TotalTimeInSeconds);
        var remainingSeconds = EditorGUILayout.Slider("Remaining Time In Seconds", m_Timer.RemainingTimeInSeconds, 0, m_Timer.TotalTimeInSeconds);

        if (EditorGUI.EndChangeCheck())
        {
            m_Timer.RemainingTimeInSeconds = remainingSeconds;
            m_Timer.TotalTimeInSeconds = totalSeconds;
            SceneView.RepaintAll();
        }
    }
}
