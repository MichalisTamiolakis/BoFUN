using BoFUN.Entities;
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

        public TaskWindow[] taskWindows = new TaskWindow[2];
        public Timer[] timers = new Timer[2];

        private Round m_AssociatedRound;
        private Trivia m_Task;

        public void InitializeWithRound(Round r)
        {
            RemoveRoundUpdateListeners();
            m_AssociatedRound = r;

            ParseTaskInfo();
            DisplayRoundInfo();
            InitializeTimers();
            AddRoundUpdateListeners();
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
            }
        }

        private void OnRoundUpdated(Round displayingRound)
        {
            // Update the timer 
            if (displayingRound.started)
            {
                foreach (Timer t in timers)
                {
                    t.RemainingTimeInSeconds = displayingRound.remainingTime;
                }
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
