using UnityEngine;

namespace BoFUN.Menu
{
    public class MenuManager : MonoBehaviour
    {
        public static MenuManager Instance
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


        public int NumberOfTeams
        {
            get => GameManager.GameManager.Instance.gameDescriptor.numberOfTeams;
            set => GameManager.GameManager.Instance.gameDescriptor.numberOfTeams = value;
        }

        public int NumberOfPlayers
        {
            get => GameManager.GameManager.Instance.gameDescriptor.numberOfPlayers;
            set => GameManager.GameManager.Instance.gameDescriptor.numberOfPlayers = value;
        }

        public bool TriviaOption
        {
            get => GameManager.GameManager.Instance.gameDescriptor.trivia;
            set => GameManager.GameManager.Instance.gameDescriptor.trivia = value;
        }

        public bool PantomimeOption
        {
            get => GameManager.GameManager.Instance.gameDescriptor.pantomime;
            set => GameManager.GameManager.Instance.gameDescriptor.pictionary = value;
        }

        public bool PictionaryOption
        {
            get => GameManager.GameManager.Instance.gameDescriptor.pictionary;
            set => GameManager.GameManager.Instance.gameDescriptor.pantomime = value;
        }

        public int TimePerRound
        {
            get => GameManager.GameManager.Instance.gameDescriptor.timePerRound;
            set => GameManager.GameManager.Instance.gameDescriptor.timePerRound = value;
        }


        public NumberOfPlayersAndTeamsSelection numberOfPlayersAndTeamsSelectionWindow;
        public GameSettings gameSettingsWindow;
        //public GameObject

        public enum MenuPage
        {
            NumberOfPlayersAndTeams = 0,
            GameSettings = 1
        }

        private MenuPage UIPage = 0;

        void Start()
        {

            // Adjust positions
            gameSettingsWindow.Window.transform.localPosition += new Vector3(1920, 0, 0);
            gameSettingsWindow.focused = false;

            numberOfPlayersAndTeamsSelectionWindow.Window.SetActive(true);
            gameSettingsWindow.Window.SetActive(true);
        }

        void Update()
        {

        }

        public void NextPage()
        {
            if (UIPage == MenuPage.NumberOfPlayersAndTeams)
            {
                UIPage++;
                numberOfPlayersAndTeamsSelectionWindow.focused = false;

                // Play animation
                numberOfPlayersAndTeamsSelectionWindow.Window.LeanMoveLocalX(numberOfPlayersAndTeamsSelectionWindow.Window.transform.localPosition.x - 1920, .5f).setEaseOutQuart();
                gameSettingsWindow.Window.LeanMoveLocalX(gameSettingsWindow.Window.transform.localPosition.x - 1920, .5f).setEaseOutQuart().setOnComplete(()=> { gameSettingsWindow.focused = true; });
            }
            else if (UIPage == MenuPage.GameSettings)
            {
                CreateGame();
            }
        }

        public void PreviousPage()
        {
            if(UIPage == MenuPage.GameSettings)
            {
                UIPage--;
                gameSettingsWindow.focused = false;

                // Play animation
                gameSettingsWindow.Window.LeanMoveLocalX(gameSettingsWindow.Window.transform.localPosition.x + 1920, .5f).setEaseOutQuart();
                numberOfPlayersAndTeamsSelectionWindow.Window.LeanMoveLocalX(numberOfPlayersAndTeamsSelectionWindow.Window.transform.localPosition.x + 1920, .5f).setEaseOutQuart().setOnComplete(() => { numberOfPlayersAndTeamsSelectionWindow.focused = true; });

            }
        }

        public void CreateGame()
        {
            Debug.Log("Creating Game...");
            throw new System.NotImplementedException();
        }

    }
}
