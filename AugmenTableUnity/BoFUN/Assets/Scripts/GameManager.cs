using System.Collections;
using UnityEngine;
using BoFUN.Entities;
using BoFUN.Utilities;
using BoFUN.Menu;

namespace BoFUN.GameManager
{
    public class GameManager : MonoBehaviour
    {
        public static GameManager Instance
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
                gameCreationDescriptor = new GameCreationDescriptor();
            }
        }

        [Tooltip("The network URL and paths for communicating with the backend")]
        public NetworkSettings networkSettings;
        [Tooltip("Adjustable game design settings such as Max number of teams and players")]
        public GameSettings gameSettings;
        
        [HideInInspector]
        public GameCreationDescriptor gameCreationDescriptor; // Before a game is created all game settings are stored here
        public Game currentGame; // The current active game;


        private bool hasGameCreated = false;

        /// <summary>
        /// Sends Create Game Request to server to create a game with the gameDescriptor data
        /// </summary>
        public void CreateGame()
        {
            hasGameCreated = true;

            //request.SetRequestHeader("appKey", "ABC");
            Debug.Log(gameCreationDescriptor.ToString());
            currentGame = null;


            HideMenuAndTransitionToTeamJoin();
            StartCoroutine(DelayCreateGame());
        }



        // =================== HELPER FUNCTIONS ==================
        // NOTE: Only needed to showcase loading screen.
        private IEnumerator DelayCreateGame() 
        {
            yield return new WaitForSeconds(2f);

            string jsonString = gameCreationDescriptor.toJSON();
            Debug.Log("Creating Game: " + jsonString);

            NetworkUtilities.Instance.Post(networkSettings.serverURL + "/" + networkSettings.gameAPI.createGamePath, jsonString,
            (bool success, string response) => {
                if (success)
                {
                    currentGame = Game.CreateFromJSON(response);
                    Debug.Log("Game created: " + currentGame.ToString());
                    TeamAssignmentManager.Instance.GenerateQRCodes();
                }
                else
                {
                    Debug.LogError("Error: " + response);
                }
            });

        }


        /// <summary>
        /// Hides Initial Menu
        /// </summary>
        private void HideMenuAndTransitionToTeamJoin()
        {
            Menu.MenuManager.Instance.ShowMenu(false);
            TeamAssignmentManager.Instance.ShowScreen(true);
        }


        // ======================= GAME LOGIC ====================

        /// <summary>
        /// Starts a new game 
        /// </summary>
        public void StartGame()
        {
            TeamAssignmentManager.Instance.ShowScreen(false);


            // show menu
            Menu.MenuManager.Instance.ShowMenu(true);
        }

        public void Start()
        {
            StartGame();
        }
    }
}
