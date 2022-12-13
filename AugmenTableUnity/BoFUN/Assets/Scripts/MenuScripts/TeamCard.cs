using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using BoFUN.Entities;
using TMPro;

public class TeamCard : MonoBehaviour
{
    //public
    [Header("Component references needed for the card to work")]
    public Image backgroundImageComponent;
    public TMP_Text teamName; 
    public Image avatarImageComponent;
    public GameObject playerEntry;

    public Team associatedTeam; // The team entity this card shows

    public Color BackgroundColor
    {
        get => backgroundImageComponent.color;
        set => backgroundImageComponent.color = value;
    }

    public Sprite Avatar
    {
        get => avatarImageComponent.sprite;
        set => avatarImageComponent.sprite = value;
    }

    private List<MenuPlayerDescriptor> m_JoinedPlayers = new List<MenuPlayerDescriptor>();

    //public void AddPlayer(MenuPlayerDescriptor player)
    //{
    //    m_JoinedPlayers.Add(player);
    //}

    //public bool RemovePlayer(int playerId)
    //{
    //    //WebSocketSharp
    //    for(int i=m_JoinedPlayers.Count-1; i>=0; i--)
    //    {
    //        if (m_JoinedPlayers[i].PlayerId == playerId)
    //        {
    //            m_JoinedPlayers.RemoveAt(i);
    //            return true;
    //        }
    //    }
    //    return false;
    //}

    // Start is called before the first frame update
    void Start()
    {
        if (!backgroundImageComponent)
        {
            Debug.Log("Incorrect Team Card setup");
            return;
        }   
    }

    /// <summary>
    /// Renders the associated team in tha card
    /// </summary>
    public void Render()
    {
        //Render name
        teamName.text = associatedTeam.name;

        // Add background card color
        if (ColorUtility.TryParseHtmlString(associatedTeam.color, out Color teamColor))
        {
            backgroundImageComponent.color = teamColor;

        }
        else
        {
            Debug.LogError("Could not parse color: " + associatedTeam.color);
        }
        
    }
}
