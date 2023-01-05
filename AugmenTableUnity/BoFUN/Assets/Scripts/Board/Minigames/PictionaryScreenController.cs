using BoFUN.Entities;
using TMPro;
using UnityEngine;

namespace BoFUN.Board.MiniGames
{
    public class PictionaryScreenController : MonoBehaviour
    {
        public PointerDraw pointerDraw;
        public Timer[] timers = new Timer[2];

        private Round m_AssociatedRound;

        public void InitializeWithRound(Round r)
        {
            RemoveRoundUpdateListeners();
            m_AssociatedRound = r;

            InitializeTimers();

            InitializeToolBox();

            pointerDraw.EraseDrawing(); // Clear previous drawing

            AddRoundUpdateListeners();
        }

        // Private Helper Functions

        private void InitializeTimers()
        {
            // Initialize timers
            foreach (Timer t in timers)
            {
                t.TotalTimeInSeconds = GameManager.GameManager.Instance.currentGame.duration;
            }
        }

        /// <summary>
        /// Positions the toolbox in front of the player about to draw
        /// </summary>
        private void InitializeToolBox()
        {
            // TODO Position the toolbox
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
