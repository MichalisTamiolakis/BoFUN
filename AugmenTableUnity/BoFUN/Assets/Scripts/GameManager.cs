using System.Collections;
using UnityEngine;
using BoFUN.Entities;
using BoFUN.Utilities;
using BoFUN.Menu;
using System.IO;

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

#if !UNITY_EDITOR
            // Read data from file in assetsream
            Debug.Log("Loading Settings from Streaming Assets...");
            StreamReader sr = new StreamReader(Application.streamingAssetsPath + "/Settings/NetworkSettings.json");
            string jsonString = sr.ReadToEnd();
            this.networkSettings = NetworkSettings.CreateFromJSON(jsonString);
            sr.Close();
            sr = new StreamReader(Application.streamingAssetsPath + "/Settings/GameSettings.json");
            jsonString = sr.ReadToEnd();
            this.gameSettings = GameSettings.CreateFromJSON(jsonString);
            sr.Close();
#endif
        }

        [Tooltip("The network URL and paths for communicating with the backend")]
        public NetworkSettings networkSettings;
        [Tooltip("Adjustable game design settings such as Max number of teams and players")]
        public GameSettings gameSettings;
        
        [HideInInspector]
        public GameCreationDescriptor gameCreationDescriptor; // Before a game is created all game settings are stored here
        public Game currentGame; // The current active game;


        public enum Screen
        {
            Idle = 0,
            GameCreationScreen = 1,
            TeamAssignmentScreen = 2,
            Board = 3
        }
        private Screen currentScreen = Screen.GameCreationScreen;


        /// <summary>
        /// Sends Create Game Request to server to create a game with the gameDescriptor data
        /// </summary>
        public void CreateGame()
        {
            //request.SetRequestHeader("appKey", "ABC");
            Debug.Log(gameCreationDescriptor.ToString());
            currentGame = null;


            StartCoroutine(DelayCreateGame());
        }

        // =================== HELPER FUNCTIONS ==================
        // NOTE: Only needed to showcase loading screen.
        private IEnumerator DelayCreateGame() 
        {
            MenuScreenManager.Instance.ShowLoading(true);

            yield return new WaitForSeconds(2f);

            string jsonString = gameCreationDescriptor.toJSON();
            Debug.Log("Creating Game: " + jsonString);

            NetworkUtilities.Instance.Post(networkSettings.serverURL + "/" + networkSettings.gameAPI.createGamePath, jsonString,
            (bool success, string response) => {
                if (success)
                {
                    currentGame = Game.CreateFromJSON(response);
                    Debug.Log("Game created: " + currentGame.ToString());
                    MenuScreenManager.Instance.ShowLoading(false);
                    NextScreen();
                }
                else
                {
                    Debug.LogError("Error: " + response);
                    MenuScreenManager.Instance.ShowLoading(false);
                }
            });
        }

        public void NextScreen()
        {
            ShowScreen((Screen)Mathf.Min((int)currentScreen + 1, 3));

        }

        public void PreviousScreen()
        {
            ShowScreen((Screen)Mathf.Max((int)currentScreen - 1, 0));
        }

        /// <summary>
        /// 
        /// </summary>
        private void ShowScreen(Screen screen)
        {

            MenuScreenManager.Instance.ShowScreen(false);
            TeamAssignmentScreenManager.Instance.ShowScreen(false);
            BoardScreenManager.Instance.ShowScreen(false);

            switch (screen)
            {
                case Screen.Idle:
                    break;
                case Screen.GameCreationScreen:
                    MenuScreenManager.Instance.ShowScreen(true);
                    break;
                case Screen.TeamAssignmentScreen:
                    TeamAssignmentScreenManager.Instance.ShowScreen(true);
                    break;
                case Screen.Board:
                    BoardScreenManager.Instance.ShowScreen(true);
                    break;
            }

            currentScreen = screen;
        }

        // ======================= GAME LOGIC ====================

        /// <summary>
        /// Starts a new game 
        /// </summary>
        public void StartGame()
        {
            ShowScreen(Screen.GameCreationScreen);
        }


        public void Start()
        {
            StartGame();
        }

        public void Update()
        {
            if (Input.GetKeyUp(KeyCode.Escape))
            {
                Application.Quit();
            }
        }
    }
}
