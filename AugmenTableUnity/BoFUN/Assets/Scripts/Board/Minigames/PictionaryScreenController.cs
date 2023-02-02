using BoFUN.Entities;
using BoFUN.Utilities;
using System;
using System.Web;
using TMPro;
using UnityEngine;

namespace BoFUN.Board.MiniGames
{
    public class PictionaryScreenController : MonoBehaviour
    {
        public PointerDraw pointerDraw;
        public Timer[] timers = new Timer[2];

        private Round m_AssociatedRound;
        private Action<Round> onMinigameFinished;

        private LTDescr timerAnimation = null;


        public void InitializeWithRound(Round r, Action<Round> onMinigameFinished)
        {
            this.onMinigameFinished = onMinigameFinished;
            RemoveRoundUpdateListeners();
            m_AssociatedRound = r;

            InitializeTimers();

            InitializeToolBox();

            pointerDraw.enabled = true;
            pointerDraw.EraseDrawing(); // Clear previous drawing

            AddRoundUpdateListeners();

            // Start sending picture to server
            InvokeRepeating("UploadDrawingToServer", 0.2f, 0.2f);

            // Start Sound
            AudioManager.Instance.PlayPictionarySound();
        }



        // Private Helper Functions
        private void InitializeTimers()
        {
            // Initialize timers
            foreach (Timer t in timers)
            {
                t.TotalTimeInSeconds = GameManager.GameManager.Instance.currentGame.duration;
                t.RemainingTimeInSeconds = t.TotalTimeInSeconds;
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

                pointerDraw.enabled = false;

                // Stop sending picture to socket
                CancelInvoke();

                RemoveRoundUpdateListeners();

                AudioManager.Instance.StopAllSounds();

                // Show win lose notification
                if (displayingRound.victory)
                {
                    string[] messages =
                    {
                        "Well done!",
                        "Nice Job!",
                        "Bravo!",
                        "Keep it up!"
                    };
                    NotificationManager.Instance.ShowCorrectNotificationSphere(messages[UnityEngine.Random.Range(0, messages.Length - 1)], 3f, () => onMinigameFinished?.Invoke(displayingRound));
                    AudioManager.Instance.PlayCorrectAnswerSound();
                }
                else
                {
                    string[] messages =
                    {
                        "<b>You've lost</b>\nTime's up."
                    };
                    NotificationManager.Instance.ShowErrorNotificationSphere(messages[UnityEngine.Random.Range(0, messages.Length - 1)], 3f, () => onMinigameFinished?.Invoke(displayingRound));
                    AudioManager.Instance.PlayTimeFinishedSound();

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
    
        private void UploadDrawingToServer()
        {

            //pointerDraw.Imag
            string drawing = HttpUtility.JavaScriptStringEncode(pointerDraw.GetDrawingSVG());

            NetworkUtilities.Instance.SocketPublish(GameManager.GameManager.Instance.networkSettings.sockets.pictionaryDrawingUpdatedEvent, drawing);
        }
    }
}
