using UnityEngine;
using BoFUN.Entities;
using System;

class TeamPositionIndicator : MonoBehaviour
{
    private Team m_AssociatedTeam;
    public Team AssociatedTeam
    {
        get
        {
            return m_AssociatedTeam;
        }
        set
        {
            m_AssociatedTeam = value;

            m_AssociatedTeam.onUpdate.AddListener((Team t)=> { Repaint(); });
        }
    }

    public int positionInBoard = 0;
    
    [SerializeField]
    private Transform childCylinder;
    [SerializeField]
    private MeshRenderer teamIndicatorRenderer;

    private Material materialInstance;


    public void Start()
    {
        materialInstance = Instantiate(teamIndicatorRenderer.material);
        teamIndicatorRenderer.material = materialInstance;
        Repaint();
    }


    public void Repaint()
    {
        if (m_AssociatedTeam == null || !teamIndicatorRenderer)
            return;

        if (ColorUtility.TryParseHtmlString(m_AssociatedTeam.color, out Color color))
        {
            materialInstance.SetColor("_BaseColor", color);
            //Debug.Log("Setting color" + color);
        }
        else
        {
            Debug.LogError("Could not parse color", this);
        }
    }


    public LTDescr Move(Vector3[] path, float time)
    {
        return LeanTween.moveSpline(gameObject, path, time).setEaseInOutQuart();
    }

    public LTDescr Move(Vector3[] path, float time, Action onMoveFinished)
    {
        return LeanTween.moveSpline(gameObject, path, time).setEaseInOutQuart().setOnComplete(onMoveFinished);
    }

    public static TeamPositionIndicator Create(Team associatedTeam, Vector3 offset)
    {
        GameObject gameObject = Instantiate(Resources.Load("Board/Prefabs/TeamPositionIndicator") as GameObject);    
    

        if (gameObject.TryGetComponent(out TeamPositionIndicator teamIndicatorScript))
        {
            teamIndicatorScript.m_AssociatedTeam = associatedTeam;
            teamIndicatorScript.childCylinder.transform.position += offset;
        }
        return teamIndicatorScript;
    }

    public void OnDestroy()
    {
        Destroy(gameObject);
    }
}
