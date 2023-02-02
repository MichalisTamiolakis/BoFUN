using BoFUN.Entities;
using BoFUN.Utilities;
using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace BoFUN.Board.MiniGames
{
    public class TriviaScreenController : MonoBehaviour
    {
        [System.Serializable]
        public struct TaskWindow
        {
            public TMP_Text titleText;
            public TMP_Text questionText;
            public Answer[] answers;
        }

        [System.Serializable]
        public struct Answer
        {
            public Image background;
            public TMP_Text text;
            public Image numberingImage;
            public TMP_Text numberingText;

            public void Show(bool show)
            {
                text.enabled = show;
                numberingImage.enabled = show;
                numberingText.enabled = show;
            }
        }

        public Color selectedAnswerColor = Color.white;
        public Color correctAnswerColor = Color.green;
        public AnimationCurve flashingAnimationCurve;

        public TaskWindow[] taskWindows = new TaskWindow[2];
        public Timer[] timers = new Timer[2];

        private Round m_AssociatedRound;
        private Trivia m_Task;
        private Action<Round> onMinigameFinished;
        private int selectedAnswer = -1;

        private LTDescr timerAnimation = null;

        public void InitializeWithRound(Round r, Action<Round> onMinigameFinished)
        {
            this.onMinigameFinished = onMinigameFinished;
            RemoveRoundUpdateListeners();
            m_AssociatedRound = r;

            ParseTaskInfo();
            DisplayRoundInfo();
            InitializeTimers();
            AddRoundUpdateListeners();

            NetworkUtilities.Instance.SocketSubscribe("TriviaSelectedAnswerChanged", OnSelectedAnswerChanged);
            selectedAnswer = -1;

            // Play ambient sound
            AudioManager.Instance.PlayTriviaSound();
        }

        // Private Helper Functions
        private void ParseTaskInfo()
        {
            m_Task = Trivia.CreateFromJSON(m_AssociatedRound.minigameJSON);
        }

        private void DisplayRoundInfo()
        {
            Team playingTeam = GameManager.GameManager.Instance.currentGame.GetTeam(m_AssociatedRound.team);

            // Initialize the text info
            foreach (TaskWindow tw in taskWindows)
            {
                tw.titleText.text = $"<b>{playingTeam.name}</b>,";
                tw.questionText.text = $"{m_Task.question}";
                
                // Hide all answers
                foreach(Answer a in tw.answers)
                {
                    a.Show(false);
                }

                // Paint only available answers
                for(int i=0; i<m_Task.answers.Length; i++)
                {
                    tw.answers[i].background.color = Color.clear;
                    tw.answers[i].text.text = m_Task.answers[i];
                    tw.answers[i].Show(true);
                }
            }
        }

        private void InitializeTimers()
        {
            // Initialize timers
            foreach (Timer t in timers)
            {
                t.TotalTimeInSeconds = GameManager.GameManager.Instance.currentGame.duration;
                t.RemainingTimeInSeconds = t.TotalTimeInSeconds;
            }
        }

        private void OnRoundUpdated(Round displayingRound)
        {
            // Update the timer 
            if (displayingRound.started && !displayingRound.ended)
            {
                
                // Start animation
                if (timerAnimation == null)
                {
                    timerAnimation = LeanTween.value(GameManager.GameManager.Instance.currentGame.duration, 0, GameManager.GameManager.Instance.currentGame.duration).setOnUpdate((float value) =>
                    {
                        UpdateTimers(value);
                    });
                }
                // Sync server time and local animation
                else
                {
                    //float animatorRemainingTime = timerAnimation.time - timerAnimation.passed;
                    float actualRemainingTime = (float)displayingRound.remainingTime;
                   
                    // Adjust for deviation
                    timerAnimation.passed = GameManager.GameManager.Instance.currentGame.duration-actualRemainingTime-1;
                }
                //lerp LeanTween.value(gameObject)
            }
            else if (displayingRound.ended)
            {
                // Cancel time animation
                LeanTween.cancel(timerAnimation.id);
                timerAnimation = null;
                UpdateTimers(displayingRound.remainingTime);
                RemoveRoundUpdateListeners();

                // Show win/lose animation
                foreach(TaskWindow tw in taskWindows)
                {
                    Color originalColorOfAnswer = tw.answers[m_Task.correctAnswer].background.color;
                    LeanTween.value(0, 1f, 2f).setEase(flashingAnimationCurve).setOnUpdate((float x)=>
                    {
                        tw.answers[m_Task.correctAnswer].background.color = Color.Lerp(originalColorOfAnswer, correctAnswerColor, x);
                    });
                }

                // Play sound
                AudioManager.Instance.StopAllSounds();
                if (displayingRound.victory)
                {
                    AudioManager.Instance.PlayCorrectAnswerSound();
                }
                else if (displayingRound.remainingTime>0)
                {
                    AudioManager.Instance.PlayIncorrectAnswerSound();
                }
                else if(displayingRound.remainingTime <= 0)
                {
                    AudioManager.Instance.PlayTimeFinishedSound();
                }



                // Back to board call
                LeanTween.delayedCall(3f, ()=> {
                    onMinigameFinished?.Invoke(displayingRound);
                });

            }
        }

        private void OnSelectedAnswerChanged(SocketEvent e)
        {
            selectedAnswer = e.GetData<int>();

            // Update answer states
            foreach (TaskWindow tw in taskWindows)
            {
                // Make clear all non selected answer, and leave only selected white
                for (int i = 0; i < m_Task.answers.Length; i++)
                {
                    if(selectedAnswer == i)
                    {
                        tw.answers[i].background.color = selectedAnswerColor;
                    }
                    else
                    {
                        tw.answers[i].background.color = Color.clear;
                    }
                }
            }
        }

        private void UpdateTimers(float remainingTime)
        {
            foreach (Timer t in timers)
            {

                t.RemainingTimeInSeconds = remainingTime;
            }
        }

        private void RemoveRoundUpdateListeners()
        {
            if (m_AssociatedRound == null)
                return;

            m_AssociatedRound.onUpdate.RemoveListener(OnRoundUpdated);
        }

        private void AddRoundUpdateListeners()
        {
            if (m_AssociatedRound == null)
                return;

            m_AssociatedRound.onUpdate.AddListener(OnRoundUpdated);
        }
    }
}
