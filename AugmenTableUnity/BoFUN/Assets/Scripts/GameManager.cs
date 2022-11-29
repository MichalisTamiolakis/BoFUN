using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

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
                gameDescriptor = new GameDescriptor();
            }
        }

        public NetworkSettings networkSettings;
        
        [HideInInspector]
        public GameDescriptor gameDescriptor;

        private bool hasGameCreated = false;

        /// <summary>
        /// Sends Create Game Request to server to create a game with the gameDescriptor data
        /// </summary>
        public void CreateGame()
        {
            hasGameCreated = true;
            Debug.Log(gameDescriptor.ToString());


            // Send data to server
            WWWForm form = new WWWForm();
            form.AddField("duration", gameDescriptor.timePerRound);
            form.AddField("totalPlayers", gameDescriptor.numberOfPlayers);
            form.AddField("totalTeams", gameDescriptor.numberOfTeams);
            form.AddField("pantomime", gameDescriptor.pantomime ? 1:0);
            form.AddField("pictionary", gameDescriptor.pictionary ? 1 : 0);
            form.AddField("trivia", gameDescriptor.trivia ? 1 : 0);


            //request.SetRequestHeader("Content-Type", "application/json");
            //request.SetRequestHeader("Accept", "text/csv");
            //request.SetRequestHeader("appKey", "ABC");

            UnityWebRequest request = UnityWebRequest.Post(networkSettings.serverURI+"/"+networkSettings.createGamePath, form);
            StartCoroutine(OnCreateGameResponse(request));

        }



        // =================== HELPER FUNCTIONS ==================
        private IEnumerator OnCreateGameResponse(UnityWebRequest req)
        {
            yield return req.SendWebRequest();

            if (req.result != UnityWebRequest.Result.Success)
            {
                Debug.Log(req.error);
            }
            else
            {
                HideMenuAndTransitionToTeamJoin();
            }

        }


        /// <summary>
        /// Hides Initial Menu
        /// </summary>
        private void HideMenuAndTransitionToTeamJoin()
        {
            Menu.MenuManager.Instance.ShowMenu(false);
        }


        // ======================= GAME LOGIC ====================

        /// <summary>
        /// Starts a game 
        /// </summary>
        public void StartGame()
        {
            // show menu
            Menu.MenuManager.Instance.ShowMenu(true);
        }

        public void Start()
        {
            StartGame();
        }
    }
}
