using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using BoFUN.Entities;
using BoFUN.Utilities;
using BoFUN.UI;
using System;

namespace BoFUN.Menu
{
    public class TeamAssignmentScreenManager : MonoBehaviour
    {
        public static TeamAssignmentScreenManager Instance
        {
            get;
            private set;
        }

        private void Awake()
        {
            // If there is an instance, and it's not me, delete myself.

            if (Instance != null && Instance != this)
            {
                Destroy(this);
            }
            else
            {
                Instance = this;
            }
        }


        public GameObject TeamJoinScreen;
        public Camera UICamera;
        public Transform content;
        public GameObject loading;
        [Space(5)]
        public Button startGameButton;
        public Button exitGameButton;
        [Space(5)]
        public Transform QRCodesLeft;
        public Transform QRCodesRight;
        public Transform QRCodesTop;
        public Transform QRCodesBottom;

        [Header("Prefabs")]
        public GameObject teamCardPrefab;
        public GameObject QRCodePrefab;


        private Dictionary<Team, TeamCard> spawnedTeamCards = new Dictionary<Team, TeamCard>();
        Dictionary<int, QRCode> seatsToQRs = new Dictionary<int, QRCode>();


        public void ShowScreen(bool show)
        {
            UICamera.enabled = show;
            TeamJoinScreen.SetActive(show);

            if (show)
            {
                Repaint();
            }
            else
            {
                Clear();
            }
        }
        public void ShowLoading(bool enabled)
        {
            loading.SetActive(enabled);
        }
        

        /// <summary>
        /// Removes all cards from the screen and enables loading
        /// </summary>
        public void Clear()
        {
            // Remove all team cards
            foreach (TeamCard tc in spawnedTeamCards.Values)
            {
                Destroy(tc);
            }
            spawnedTeamCards.Clear();
        }

        public void Repaint()
        {
            PaintQRCodes();


            RepaintTeamCards();

            // Enable/Disable buttons
            UpdateButtonStates();

        }
        
        
        // Sockets
        public void Start()
        {
            // Set up all socket realtime updates
            SocketEventHandler.Instance.seatOccupiedEvent.AddListener(DisableSeatQR);
            SocketEventHandler.Instance.teamUpdatedEvent.AddListener((Team t) => { UpdateButtonStates(); });
        }

        public void DisableSeatQR(int seatNo)
        {
            if(seatsToQRs.TryGetValue(seatNo, out QRCode code))
            {
                code.SetScannable(false);
            }
        }

        // ========== Private Functions ==========
        
        public void PaintQRCodes()
        {
            //Remove any previous qr codes
            foreach(QRCode code in seatsToQRs.Values)
            {
                Destroy(code);
            }
            seatsToQRs.Clear();

            int qrCodesRemaining = GameManager.GameManager.Instance.gameCreationDescriptor.totalPlayers;

            // Top And Bottom QRs
            int topAndBottom = Mathf.FloorToInt(qrCodesRemaining * (3f / 5f));
            int leftAndRight = Mathf.CeilToInt(qrCodesRemaining * (2f / 5f));

            int bottomQRs = Mathf.CeilToInt(topAndBottom / 2f);
            int topQRs = Mathf.FloorToInt(topAndBottom / 2f);

            int rightQRs = Mathf.CeilToInt(leftAndRight / 2f);
            int leftQRs = Mathf.FloorToInt(leftAndRight / 2f);

            int seat = 0;
            // Bottom
            for(int i=0; i< bottomQRs; i++)
            {
                QRCode code = QRCode.Create(GameManager.GameManager.Instance.networkSettings.frontendURL + "/" + GameManager.GameManager.Instance.networkSettings.frontEnd.GetJoinPagePath(seat));
                code.transform.SetParent(QRCodesBottom, false);
                seatsToQRs[seat]= code;
                seat++;
            }

            // Right
            for (int i = 0; i < rightQRs; i++)
            {
                QRCode code = QRCode.Create(GameManager.GameManager.Instance.networkSettings.frontendURL + "/" + GameManager.GameManager.Instance.networkSettings.frontEnd.GetJoinPagePath(seat));
                code.transform.SetParent(QRCodesRight, false);
                seatsToQRs[seat] = code;
                seat++;
            }

            // Top
            for (int i = 0; i < topQRs; i++)
            {
                QRCode code = QRCode.Create(GameManager.GameManager.Instance.networkSettings.frontendURL + "/" + GameManager.GameManager.Instance.networkSettings.frontEnd.GetJoinPagePath(seat));
                code.transform.SetParent(QRCodesTop, false);
                seatsToQRs[seat] = code;
                seat++;
            }

            // Left
            for (int i = 0; i < leftQRs; i++)
            {
                QRCode code = QRCode.Create(GameManager.GameManager.Instance.networkSettings.frontendURL + "/" + GameManager.GameManager.Instance.networkSettings.frontEnd.GetJoinPagePath(seat));
                code.transform.SetParent(QRCodesLeft, false);
                seatsToQRs[seat] = code;
                seat++;
            }
        }
        
        private void RepaintTeamCards()
        {
            // Remove old Cards
            Clear();
            // Spawn new cards
            foreach (Team t in GameManager.GameManager.Instance.currentGame.teams)
            {
                if (!spawnedTeamCards.ContainsKey(t))
                {
                    GameObject card = Instantiate(teamCardPrefab);
                    card.transform.SetParent(content, false);

                    if (card.TryGetComponent(out TeamCard tc))
                    {
                        tc.AssociatedTeam = t;
                    }
                    else
                    {
                        DestroyImmediate(card);
                    }

                    tc.Repaint();
                    spawnedTeamCards.Add(t, tc);
                }
            }
        }

        private void UpdateButtonStates()
        {
            bool canStartGame = true;

            int totalPlayersInTeams = 0;

            if (GameManager.GameManager.Instance.currentGame != null && GameManager.GameManager.Instance.currentGame.teams != null)
            {
                foreach (Team t in GameManager.GameManager.Instance.currentGame.teams)
                {
                    totalPlayersInTeams += t.members.Length;
                    if (t.members.Length < GameManager.GameManager.Instance.gameCreationDescriptor.minPlayersPerTeam)
                    {
                        canStartGame = false;
                        break;
                    }
                    else if (t.members.Length > GameManager.GameManager.Instance.gameCreationDescriptor.maxPlayersPerTeam)
                    {
                        canStartGame = false;
                        break;
                    }
                }

                // TODO: Do we need the game to be able to start without all the players? If yes, remove this if statement
                if (totalPlayersInTeams != GameManager.GameManager.Instance.gameCreationDescriptor.totalPlayers)
                {
                    canStartGame = false;
                }
            }
            else
            {
                canStartGame = false;
            }


            startGameButton.interactable = canStartGame;
            exitGameButton.interactable = true;
        }
    }
}