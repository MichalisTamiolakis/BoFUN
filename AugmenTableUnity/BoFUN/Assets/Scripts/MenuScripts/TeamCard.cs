using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TeamCard : MonoBehaviour
{
    //public
    [Header("Component references needed for the card to work")]
    public Image backgroundImageComponent;
    public Image avatarImageComponent;
    public GameObject playerEntry;

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

    public void AddPlayer(MenuPlayerDescriptor player)
    {
        m_JoinedPlayers.Add(player);
    }

    public bool RemovePlayer(int playerId)
    {
        //WebSocketSharp
        for(int i=m_JoinedPlayers.Count-1; i>=0; i--)
        {
            if (m_JoinedPlayers[i].PlayerId == playerId)
            {
                m_JoinedPlayers.RemoveAt(i);
                return true;
            }
        }
        return false;
    }

    // Start is called before the first frame update
    void Start()
    {
        if (!backgroundImageComponent)
        {
            Debug.Log("Incorrect Team Card setup");
            return;
        }   
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
