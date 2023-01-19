using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BoFUN.GameManager;
using BoFUN.Entities;

public class Step : MonoBehaviour
{
    public enum StepTypes
    {
        Pantomime = 0,
        Trivia = 1,
        Pictionary = 2,
        Start = 3,
        End = 4
    }

    [SerializeField]
    private StepTypes m_StepType = 0;
    [SerializeField]
    private MeshRenderer m_Renderer;

    /// <summary>
    /// The materials for the steps:
    /// 0:Pantomime
    /// 1:Trivia
    /// 2:Pictionary
    /// 3:Start
    /// 4:End
    /// </summary>
    public Material[] materials = new Material[5];

    public StepTypes StepType{
        get => m_StepType;
        set
        {
            m_StepType = value;
            if (m_Renderer)
            {
                m_Renderer.material = materials[(int)m_StepType];
            }
        }
    }

    public void AssignRandomStepType()
    {
        if ((int)m_StepType < 3)
        {
            List<int> choices = new List<int>();
            if (GameManager.Instance.currentGame.pantomime)
                choices.Add(0);
            if (GameManager.Instance.currentGame.trivia)
                choices.Add(1);
            if (GameManager.Instance.currentGame.pictionary)
                choices.Add(2);

            if (choices.Count <= 0)
                return;

            this.StepType = (StepTypes)choices[Mathf.RoundToInt(Random.Range(0f, (float)choices.Count-1))];

        }
    }

    public MiniGame GetMinigame()
    {
        if ((int)m_StepType < 3)
        {
            return (MiniGame)((int)m_StepType);
        }
        else
        {
            List<int> choices = new List<int>();
            if (GameManager.Instance.currentGame.pantomime)
                choices.Add(0);
            if (GameManager.Instance.currentGame.trivia)
                choices.Add(1);
            if (GameManager.Instance.currentGame.pictionary)
                choices.Add(2);

            return (MiniGame)choices[Mathf.RoundToInt(Random.Range(0f, (float)choices.Count - 1))];
        }
    }

    private void Reset()
    {
        TryGetComponent(out m_Renderer);
    }

    [ContextMenu("Apply Correct Material")]
    public void ApplyMaterial()
    {
        StepType = m_StepType;
    }

}
