using BoFUN.Entities;
using TMPro;
using UnityEngine;

namespace BoFUN.Board.MiniGames {
    public class PantomimeScreenController : MonoBehaviour
    {
        [System.Serializable]
        public struct TaskWindow
        {
            public TMP_Text titleText;
            public TMP_Text teamText;
            public TMP_Text categoryText;
            public TMP_Text playerText;
        }

        public TaskWindow[] taskWindows = new TaskWindow[2];
        public Timer[] timers = new Timer[2];

        private Round m_AssociatedRound;
        private Pantomime m_Task;

        private LTDescr timerAnimation = null;


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
            m_Task = Pantomime.CreateFromJSON(m_AssociatedRound.minigameJSON);
        }

        private void DisplayRoundInfo()
        {
            Team playingTeam = GameManager.GameManager.Instance.currentGame.GetTeam(m_AssociatedRound.team);
            Player playingPlayer = GameManager.GameManager.Instance.currentGame.GetPlayer(m_AssociatedRound.player);


            // Initialize the text info
            foreach (TaskWindow tw in taskWindows)
            {
                tw.titleText.text = "Pantomime";
                tw.teamText.text = $"{playingTeam.name}";
                tw.categoryText.text = $"{m_Task.category}";
                tw.playerText.text = $"{playingPlayer.username}";
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
                    timerAnimation.passed = GameManager.GameManager.Instance.currentGame.duration - actualRemainingTime - 1;
                }
                //lerp LeanTween.value(gameObject)
            }
            else if (displayingRound.ended)
            {
                // Cancel time animation
                LeanTween.cancel(timerAnimation.id);
                timerAnimation = null;
                UpdateTimers(displayingRound.remainingTime);
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
