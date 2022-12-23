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
        public Transform playerEntriesParent;
        public GameObject waitingPlayers;

        private Team m_AssociatedTeam; // The team entity this card shows

        public Team AssociatedTeam
        {
            set{
                if (m_AssociatedTeam!=null)
                {
                    m_AssociatedTeam.onUpdate.RemoveListener(TeamUpdateHandler);
                }

                m_AssociatedTeam = value;

                m_AssociatedTeam.onUpdate.AddListener(TeamUpdateHandler);
            }
            get
            {
                return m_AssociatedTeam;
            }
        }

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

        public void TeamUpdateHandler(Team t)
        {
            Repaint();
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


        private Dictionary<Player, TeamCardPlayerEntry> spanwedPlayers = new Dictionary<Player, TeamCardPlayerEntry>();

        /// <summary>
        /// Renders the associated team in tha card
        /// </summary>
        public void Repaint()
        {
            //Render name
            teamName.text = m_AssociatedTeam.name;

            // Add background card color
            if (ColorUtility.TryParseHtmlString(m_AssociatedTeam.color, out Color teamColor))
            {
                backgroundImageComponent.color = teamColor;
            }

            // Destroy old player entries
            foreach(TeamCardPlayerEntry e in spanwedPlayers.Values)
            {
                Destroy(e);
            }
            spanwedPlayers.Clear();

            foreach(int playerId in m_AssociatedTeam.members)
            {
                Player p = GameManager.GameManager.Instance.currentGame.GetPlayer(playerId);
                if (p!=null) {
                    TeamCardPlayerEntry pe = TeamCardPlayerEntry.Create(p);
                    pe.transform.SetParent(playerEntriesParent, false);
                    pe.Repaint();
                    spanwedPlayers[p] = pe;
                }

            }

            // Show waiting players to join only when no player has joined
            waitingPlayers.SetActive(spanwedPlayers.Count == 0);
        }

        public void OnDestroy()
        {
            Destroy(gameObject); // Remove the whole gameobject
            m_AssociatedTeam.onUpdate.RemoveListener(TeamUpdateHandler);
        }
    }
}