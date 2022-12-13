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
            get => GameManager.GameManager.Instance.gameCreationDescriptor.numberOfTeams;
            set => GameManager.GameManager.Instance.gameCreationDescriptor.numberOfTeams = value;
        }

        public int NumberOfPlayers
        {
            get => GameManager.GameManager.Instance.gameCreationDescriptor.numberOfPlayers;
            set => GameManager.GameManager.Instance.gameCreationDescriptor.numberOfPlayers = value;
        }

        public bool TriviaOption
        {
            get => GameManager.GameManager.Instance.gameCreationDescriptor.trivia;
            set => GameManager.GameManager.Instance.gameCreationDescriptor.trivia = value;
        }

        public bool PantomimeOption
        {
            get => GameManager.GameManager.Instance.gameCreationDescriptor.pantomime;
            set => GameManager.GameManager.Instance.gameCreationDescriptor.pictionary = value;
        }

        public bool PictionaryOption
        {
            get => GameManager.GameManager.Instance.gameCreationDescriptor.pictionary;
            set => GameManager.GameManager.Instance.gameCreationDescriptor.pantomime = value;
        }

        public int TimePerRound
        {
            get => GameManager.GameManager.Instance.gameCreationDescriptor.timePerRound;
            set => GameManager.GameManager.Instance.gameCreationDescriptor.timePerRound = value;
        }

        public GameObject menuWindow;
        public NumberOfPlayersAndTeamsSelection numberOfPlayersAndTeamsSelectionPanel;
        public GameSettings gameSettingsPanel;
        //public 
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
            gameSettingsPanel.Window.transform.localPosition += new Vector3(1920, 0, 0);
            gameSettingsPanel.focused = false;

            numberOfPlayersAndTeamsSelectionPanel.Window.SetActive(true);
            gameSettingsPanel.Window.SetActive(true);
        }

        void Update()
        {

        }

        public void ShowMenu(bool show)
        {
            menuWindow.SetActive(show);
        }

        public void NextPage()
        {
            if (UIPage == MenuPage.NumberOfPlayersAndTeams)
            {
                UIPage++;
                numberOfPlayersAndTeamsSelectionPanel.focused = false;

                // Play animation
                numberOfPlayersAndTeamsSelectionPanel.Window.LeanMoveLocalX(numberOfPlayersAndTeamsSelectionPanel.Window.transform.localPosition.x - 1920, .5f).setEaseOutQuart();
                gameSettingsPanel.Window.LeanMoveLocalX(gameSettingsPanel.Window.transform.localPosition.x - 1920, .5f).setEaseOutQuart().setOnComplete(()=> { gameSettingsPanel.focused = true; });
            }
            else
            {
                Debug.LogWarning("Menu Manager no next page");
            }
        }

        public void PreviousPage()
        {
            if(UIPage == MenuPage.GameSettings)
            {
                UIPage--;
                gameSettingsPanel.focused = false;

                // Play animation
                gameSettingsPanel.Window.LeanMoveLocalX(gameSettingsPanel.Window.transform.localPosition.x + 1920, .5f).setEaseOutQuart();
                numberOfPlayersAndTeamsSelectionPanel.Window.LeanMoveLocalX(numberOfPlayersAndTeamsSelectionPanel.Window.transform.localPosition.x + 1920, .5f).setEaseOutQuart().setOnComplete(() => { numberOfPlayersAndTeamsSelectionPanel.focused = true; });
            }
            else
            {
                Debug.LogWarning("Menu Manager no previous page");
            }
        }

        public void GoToPage(MenuPage menupage)
        {
            if(menupage == MenuPage.GameSettings)
            {
                NextPage();
            }
            else if(menupage == MenuPage.NumberOfPlayersAndTeams)
            {
                PreviousPage();
            }
        }

        public void ShowLoading(bool show)
        {

        }

        public void CreateGame()
        {
            Debug.Log("Creating Game...");
            GameManager.GameManager.Instance.CreateGame();
        }

    }
}
