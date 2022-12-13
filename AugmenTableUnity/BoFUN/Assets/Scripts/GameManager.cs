using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using BoFUN.Entities;

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

        public NetworkSettings networkSettings;
        
        [HideInInspector]
        public GameCreationDescriptor gameCreationDescriptor; // Before a game is created all game settings are stored here
        public Game game; // The current active game;

        private bool hasGameCreated = false;

        /// <summary>
        /// Sends Create Game Request to server to create a game with the gameDescriptor data
        /// </summary>
        public void CreateGame()
        {
            hasGameCreated = true;


            // Send data to server
            WWWForm form = new WWWForm();
            form.AddField("duration", gameCreationDescriptor.timePerRound);
            form.AddField("totalPlayers", gameCreationDescriptor.numberOfPlayers);
            form.AddField("totalTeams", gameCreationDescriptor.numberOfTeams);
            form.AddField("pantomime", gameCreationDescriptor.pantomime ? 1:0);
            form.AddField("pictionary", gameCreationDescriptor.pictionary ? 1 : 0);
            form.AddField("trivia", gameCreationDescriptor.trivia ? 1 : 0);


            //request.SetRequestHeader("Content-Type", "application/json");
            //request.SetRequestHeader("Accept", "text/csv");
            //request.SetRequestHeader("appKey", "ABC");
            Debug.Log(gameCreationDescriptor.ToString());
            game = null;
            Debug.Log("Creating Game...");
            UnityWebRequest request = UnityWebRequest.Post(networkSettings.serverURL+"/"+networkSettings.gameAPI.createGamePath, form);
            HideMenuAndTransitionToTeamJoin();
            StartCoroutine(OnCreateGameResponse(request));

        }



        // =================== HELPER FUNCTIONS ==================
        private IEnumerator OnCreateGameResponse(UnityWebRequest req)
        {
            yield return new WaitForSeconds(2f); // NOTE: Only needed to showcase loading screen.
            yield return req.SendWebRequest();

            if (req.result != UnityWebRequest.Result.Success)
            {
                Debug.Log(req.error);
            }
            else
            {
                Debug.Log("Game created:");
                string jsonResponse = req.downloadHandler.text;
                game = Game.CreateFromJSON(jsonResponse);
                Debug.Log(game.ToString());
                
            }

        }


        /// <summary>
        /// Hides Initial Menu
        /// </summary>
        private void HideMenuAndTransitionToTeamJoin()
        {
            Menu.MenuManager.Instance.ShowMenu(false);
            TeamAssignmentManager.Instance.ShowScreen(true);
            TeamAssignmentManager.Instance.StartUpdating();
        }


        // ======================= GAME LOGIC ====================

        /// <summary>
        /// Starts a new game 
        /// </summary>
        public void StartGame()
        {
            TeamAssignmentManager.Instance.ShowScreen(false);
            TeamAssignmentManager.Instance.StopUpdating();


            // show menu
            Menu.MenuManager.Instance.ShowMenu(true);
        }

        public void Start()
        {
            StartGame();
        }
    }
}
