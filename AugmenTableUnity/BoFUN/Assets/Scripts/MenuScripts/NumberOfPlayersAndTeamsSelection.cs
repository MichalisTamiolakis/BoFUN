using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace BoFUN.Menu
{
    public class NumberOfPlayersAndTeamsSelection : MonoBehaviour
    {
        public TMP_Text numberOfTeamsText;
        public Button incrementTeams;
        public Button decrementTeams;

        public TMP_Text numberOfPlayersText;
        public Button incrementPlayers;
        public Button decrementPlayers;

        public Button next;


        [Space(10)]

        public int minNumberOfTeams = 2;
        public int maxNumberOfTeams = 4;
        public int minNumberOfPlayersPerTeam = 2;
        public int maxNumberOfPlayersPerTeam = 5;

        [HideInInspector]
        public bool focused = true;

        public GameObject Window
        {
            get => gameObject;
        }

        private int NumberOfTeams
        {
            get => MenuManager.Instance.NumberOfTeams;
            set
            {
                MenuManager.Instance.NumberOfTeams = Mathf.Clamp(value, minNumberOfTeams, maxNumberOfTeams);

                // If min num of teams, disable - button
                if (MenuManager.Instance.NumberOfTeams <= minNumberOfTeams)
                {
                    decrementTeams.interactable = false;
                    incrementTeams.interactable = true;
                }
                // If max num of teams, disable + button        
                else if (MenuManager.Instance.NumberOfTeams >= maxNumberOfTeams)
                {
                    decrementTeams.interactable = true;
                    incrementTeams.interactable = false;
                }
                // Else enable both buttons
                else
                {
                    decrementTeams.interactable = true;
                    incrementTeams.interactable = true;
                }

                // Update Display
                numberOfTeamsText.text = MenuManager.Instance.NumberOfTeams.ToString();

                // Update number of players to match the change of number of teams
                NumberOfPlayers = NumberOfPlayers;
            }
        }

        private int NumberOfPlayers
        {
            get => MenuManager.Instance.NumberOfPlayers;
            set
            {
                int minNumberOfPlayers = MenuManager.Instance.NumberOfTeams * minNumberOfPlayersPerTeam;
                int maxNumberOfPlayers = MenuManager.Instance.NumberOfTeams * maxNumberOfPlayersPerTeam;

                MenuManager.Instance.NumberOfPlayers = Mathf.Clamp(value, minNumberOfPlayers, maxNumberOfPlayers);

                // If min num of teams, disable - button
                if (MenuManager.Instance.NumberOfPlayers <= minNumberOfPlayers)
                {
                    decrementPlayers.interactable = false;
                    incrementPlayers.interactable = true;
                }
                // If max num of teams, disable + button        
                else if (MenuManager.Instance.NumberOfPlayers >= maxNumberOfPlayers)
                {
                    decrementPlayers.interactable = true;
                    incrementPlayers.interactable = false;
                }
                // Else enable both buttons
                else
                {
                    decrementPlayers.interactable = true;
                    incrementPlayers.interactable = true;
                }

                // Update Display
                numberOfPlayersText.text = MenuManager.Instance.NumberOfPlayers.ToString();
            }
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
            next.onClick.AddListener(() => { if (focused) MenuManager.Instance.NextPage(); });


            // Update number of teams and players displays;
            NumberOfTeams = NumberOfTeams;
        }

    }
}