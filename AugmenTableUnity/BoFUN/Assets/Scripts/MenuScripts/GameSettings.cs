using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using BoFUN.UI;
using TMPro;

namespace BoFUN.Menu {

    public class GameSettings : MonoBehaviour
    {
        public ToggleSlider triviaToggle;
        public ToggleSlider pictionaryToggle;
        public ToggleSlider pantomimeToggle;
        public TMP_Text timerText;
        public Button timerIncreaseButton;
        public Button timerDecreaseButton;

        public Button start;
        public Button previous;

        [Space(10)]
        public int minTimePerRound = 30; // 30 Seconds Min
        public int maxTimePerRound = 300; // 5 Minutes Max

        [HideInInspector]
        public bool focused = false;

        public GameObject Window
        {
            get => gameObject;
        }

        public int TimePerRound
        {
            get => MenuManager.Instance.TimePerRound;
            set
            {
                MenuManager.Instance.TimePerRound = Mathf.Clamp(value, minTimePerRound, maxTimePerRound);

                // If min num of teams, disable - button
                if (MenuManager.Instance.TimePerRound <= minTimePerRound)
                {
                    timerDecreaseButton.interactable = false;
                    timerIncreaseButton.interactable = true;
                }
                // If max num of teams, disable + button        
                else if (MenuManager.Instance.TimePerRound >= maxTimePerRound)
                {
                    timerDecreaseButton.interactable = true;
                    timerIncreaseButton.interactable = false;
                }
                // Else enable both buttons
                else
                {
                    timerDecreaseButton.interactable = true;
                    timerIncreaseButton.interactable = true;
                }

                int timeMinutes = MenuManager.Instance.TimePerRound / 60;
                int timeSeconds = MenuManager.Instance.TimePerRound - timeMinutes * 60;

                timerText.text = $"{timeMinutes:0}:{timeSeconds:00}";
            }
        }

        public bool TriviaOption
        {
            get => MenuManager.Instance.TriviaOption;
            set
            {
                MenuManager.Instance.TriviaOption = value;
                CheckDisableStart();
            }
        }

        public bool PantomimeOption
        {
            get => MenuManager.Instance.PantomimeOption;
            set
            {
                MenuManager.Instance.PantomimeOption = value;
                CheckDisableStart();
            }
        }

        public bool PictionaryOption
        {
            get => MenuManager.Instance.PictionaryOption;
            set
            {
                MenuManager.Instance.PictionaryOption = value;
                CheckDisableStart();
            }
        }

        /// <summary>
        /// Disables Start Button when invalid options
        /// </summary>
        private void CheckDisableStart()
        {
            if (!(PantomimeOption || TriviaOption || PictionaryOption))
            {
                start.interactable = false;
            }
            else
            {
                start.interactable = true;
            }

        }

        void Start()
        {
            if(!triviaToggle || !pantomimeToggle || !pictionaryToggle || !timerIncreaseButton || !timerDecreaseButton || !timerText || !start || !previous)
            {
                Debug.LogError("Incorrect Game Settings window setup");
                return;
            }


            // Attach Listeners
            triviaToggle.onToggle.AddListener((bool value) => { if (focused) TriviaOption = value; });
            pantomimeToggle.onToggle.AddListener((bool value) => { if (focused) PantomimeOption = value; });
            pictionaryToggle.onToggle.AddListener((bool value) => { if (focused) PictionaryOption = value; });
            timerIncreaseButton.onClick.AddListener(()=> { if (focused) TimePerRound = TimePerRound + 15; });
            timerDecreaseButton.onClick.AddListener(() => { if (focused) TimePerRound = TimePerRound - 15; });
            start.onClick.AddListener(() => { if (focused) MenuManager.Instance.CreateGame(); });
            previous.onClick.AddListener(() => { if (focused) MenuManager.Instance.PreviousPage(); });


            // Initialize UI values
            triviaToggle.Value = TriviaOption;
            pantomimeToggle.Value = PantomimeOption;
            pictionaryToggle.Value = PictionaryOption;
            TimePerRound = TimePerRound;
            CheckDisableStart();
        }
    }
}