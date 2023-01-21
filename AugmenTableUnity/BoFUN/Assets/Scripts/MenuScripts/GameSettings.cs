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

        public GameObject loadingScreen;

        [HideInInspector]
        public bool focused = false;

        public GameObject Window
        {
            get => gameObject;
        }

        public int TimePerRound
        {
            get => MenuScreenManager.Instance.TimePerRound;
            set
            {
                MenuScreenManager.Instance.TimePerRound = Mathf.Clamp(value, GameManager.GameManager.Instance.gameSettings.minRoundDurationSeconds, GameManager.GameManager.Instance.gameSettings.maxRoundDurationSeconds);

                // If min num of teams, disable - button
                if (MenuScreenManager.Instance.TimePerRound <= GameManager.GameManager.Instance.gameSettings.minRoundDurationSeconds)
                {
                    timerDecreaseButton.interactable = false;
                    timerIncreaseButton.interactable = true;
                }
                // If max num of teams, disable + button        
                else if (MenuScreenManager.Instance.TimePerRound >= GameManager.GameManager.Instance.gameSettings.maxRoundDurationSeconds)
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

                int timeMinutes = MenuScreenManager.Instance.TimePerRound / 60;
                int timeSeconds = MenuScreenManager.Instance.TimePerRound - timeMinutes * 60;

                timerText.text = $"{timeMinutes:0}:{timeSeconds:00}";
            }
        }

        public bool TriviaOption
        {
            get => MenuScreenManager.Instance.TriviaOption;
            set
            {
                MenuScreenManager.Instance.TriviaOption = value;
                CheckDisableStart();
            }
        }

        public bool PantomimeOption
        {
            get => MenuScreenManager.Instance.PantomimeOption;
            set
            {
                MenuScreenManager.Instance.PantomimeOption = value;
                CheckDisableStart();
            }
        }

        public bool PictionaryOption
        {
            get => MenuScreenManager.Instance.PictionaryOption;
            set
            {
                MenuScreenManager.Instance.PictionaryOption = value;
                CheckDisableStart();
            }
        }

        public void Repaint()
        {
            TimePerRound = TimePerRound;
            TriviaOption = TriviaOption;
            PictionaryOption = PictionaryOption;
            PantomimeOption = PantomimeOption;
        }

        public void ShowLoading(bool show)
        {
            this.loadingScreen.SetActive(show);
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
            timerIncreaseButton.onClick.AddListener(()=> { if (focused) TimePerRound = TimePerRound-(TimePerRound%15) + 15; });
            timerDecreaseButton.onClick.AddListener(() => { if (focused) TimePerRound = TimePerRound - (TimePerRound % 15) - 15; });
            start.onClick.AddListener(() => { if (focused) MenuScreenManager.Instance.CreateGame(); });
            previous.onClick.AddListener(() => { if (focused) MenuScreenManager.Instance.PreviousPage(); });


            // Initialize UI values
            triviaToggle.Value = TriviaOption;
            pantomimeToggle.Value = PantomimeOption;
            pictionaryToggle.Value = PictionaryOption;
            TimePerRound = TimePerRound;
            CheckDisableStart();
        }
    }
}