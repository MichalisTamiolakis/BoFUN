using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using BoFUN.GameManager;

namespace BoFUN.Menu
{
    public class NumberOfPlayersAndTeamsSelection : MonoBehaviour
    {
        public TMP_Text numberOfTeamsText;
        public Button incrementTeams;
        public Button decrementTeams;
        [Space(5)]
        public TMP_Text numberOfPlayersText;
        public Button incrementPlayers;
        public Button decrementPlayers;
        
        [Space(5)]
        public GameObject loadingScreen;

        [Space(5)]
        public Button next;


        [HideInInspector]
        public bool focused = true;

        public GameObject Window
        {
            get => gameObject;
        }

        private int NumberOfTeams
        {
            get => MenuScreenManager.Instance.NumberOfTeams;
            set
            {
                MenuScreenManager.Instance.NumberOfTeams = Mathf.Clamp(value, GameManager.GameManager.Instance.gameSettings.minNumberOfTeams, GameManager.GameManager.Instance.gameSettings.maxNumberOfTeams);

                // Enable Both Buttons Initally
                decrementTeams.interactable = true;
                incrementTeams.interactable = true;

                // If min num of teams, disable - button
                if (MenuScreenManager.Instance.NumberOfTeams <= GameManager.GameManager.Instance.gameSettings.minNumberOfTeams)
                {
                    decrementTeams.interactable = false;
                }
                // If max num of teams, disable + button        
                if (MenuScreenManager.Instance.NumberOfTeams >= GameManager.GameManager.Instance.gameSettings.maxNumberOfTeams)
                {
                    incrementTeams.interactable = false;
                }

                // Update Display
                numberOfTeamsText.text = MenuScreenManager.Instance.NumberOfTeams.ToString();

                // Update number of players to match the change of number of teams
                NumberOfPlayers = NumberOfPlayers;
            }
        }

        private int NumberOfPlayers
        {
            get => MenuScreenManager.Instance.NumberOfPlayers;
            set
            {
                int minNumberOfPlayers = MenuScreenManager.Instance.NumberOfTeams * GameManager.GameManager.Instance.gameSettings.minNumberOfPlayersPerTeam;
                int maxNumberOfPlayers = Mathf.Min(MenuScreenManager.Instance.NumberOfTeams * GameManager.GameManager.Instance.gameSettings.maxNumberOfPlayersPerTeam, GameManager.GameManager.Instance.gameSettings.maxNumberOfPlayersTotal);

                MenuScreenManager.Instance.NumberOfPlayers = Mathf.Clamp(value, minNumberOfPlayers, maxNumberOfPlayers);

                // Enable Both Buttons Initially
                decrementPlayers.interactable = true;
                incrementPlayers.interactable = true;

                // If min num of teams, disable - button
                if (MenuScreenManager.Instance.NumberOfPlayers <= minNumberOfPlayers)
                {
                    decrementPlayers.interactable = false;
                }
                // If max num of teams, disable + button        
                if (MenuScreenManager.Instance.NumberOfPlayers >= maxNumberOfPlayers)
                {
                    incrementPlayers.interactable = false;
                }

                // Update Display
                numberOfPlayersText.text = MenuScreenManager.Instance.NumberOfPlayers.ToString();
            }
        }

        public void ShowLoading(bool show)
        {
            this.loadingScreen.SetActive(show);
        }

        // Start is called before the first frame update
        void Start()
        {
            if (!numberOfTeamsText || !incrementTeams || !decrementTeams || !numberOfPlayersText || !incrementPlayers || !decrementPlayers || !next)
            {
                Debug.LogError("Incorrect Number Of Players and Teams window setup");
                return;
            }

            // Attach Listeners
            incrementTeams.onClick.AddListener(() => { if(focused) NumberOfTeams++; });
            decrementTeams.onClick.AddListener(() => { if (focused) NumberOfTeams--; });
            incrementPlayers.onClick.AddListener(() => { if (focused) NumberOfPlayers++; });
            decrementPlayers.onClick.AddListener(() => { if (focused) NumberOfPlayers--; });
            next.onClick.AddListener(() => { if (focused) MenuScreenManager.Instance.NextPage(); });


            // Update number of teams and players displays;
            NumberOfTeams = NumberOfTeams;
        }

    }
}