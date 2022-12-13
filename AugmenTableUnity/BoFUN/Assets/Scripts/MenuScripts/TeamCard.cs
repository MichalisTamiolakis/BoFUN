using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using BoFUN.Entities;
using TMPro;

namespace BoFUN.Menu
{
    public class TeamCard : MonoBehaviour
    {
        //public
        [Header("Component references needed for the card to work")]
        public Image backgroundImageComponent;
        public TMP_Text teamName;
        public Image avatarImageComponent;
        public GameObject playerEntry;
        public GameObject waitingPlayers;

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


        private Dictionary<Player, PlayerDescription> spanwedPlayers = new Dictionary<Player, PlayerDescription>();
        /// <summary>
        /// Renders the associated team in tha card
        /// </summary>
        public void Repaint()
        {
            //Render name
            teamName.text = associatedTeam.name;

            // Add background card color
            if (ColorUtility.TryParseHtmlString(associatedTeam.color, out Color teamColor))
            {
                backgroundImageComponent.color = teamColor;
            }

            // Remove any extra players
            //for ()


            // Show waiting players to join only when no player has joined
            waitingPlayers.SetActive(spanwedPlayers.Count == 0);
        }

        public void Destroy()
        {
            Destroy(gameObject);
        }
    }
}