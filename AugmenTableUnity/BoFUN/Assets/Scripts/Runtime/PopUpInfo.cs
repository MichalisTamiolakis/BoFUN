using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace BoFUN.Utilities
{
    public class PopUpInfo:MonoBehaviour
    {

        public GameObject upperPanel;
        public Image upperImage;
        public TMP_Text upperTitleText;
        public TMP_Text upperContentText;
        LTDescr upperAnim;
        
        [Space(10)]
        public GameObject lowerPanel;
        public Image lowerImage;
        public TMP_Text lowerTitleText;
        public TMP_Text lowerContentText;
        LTDescr lowerAnim;

        [Space(10)]
        public Image fadePanel;
        LTDescr fadeAnim;

        private const int notificationBarHeight = 250;


        /// <summary>
        /// Shows the requested notification 
        /// </summary>
        /// <param name="title"></param>
        /// <param name="content"></param>
        /// <param name="backgroundColor"></param>
        /// <param name="duration"></param>
        /// <param name="onNotificationFinished"></param>
        /// <returns></returns>
        public void ShowNotification(string title, string content, Color backgroundColor, float animationDuration, System.Action onAnimationFinished)
        {
            // Set Colors and Text
            upperImage.sprite = null;
            lowerImage.sprite = null;
            upperImage.color = backgroundColor;
            lowerImage.color = backgroundColor;
            upperTitleText.text = title;
            lowerTitleText.text = title;
            upperContentText.text = content;
            lowerContentText.text = content;

            PlayOpenAnimation(animationDuration, onAnimationFinished);
        }

        public void ShowNotification(string title, string content, Sprite backgroundImage, float animationDuration, System.Action onAnimationFinished)
        {
            // Set Sprite and Text
            upperImage.sprite = backgroundImage;
            lowerImage.sprite = backgroundImage;
            upperImage.color = Color.white;
            lowerImage.color = Color.white;
            upperTitleText.text = title;
            lowerTitleText.text = title;
            upperContentText.text = content;
            lowerContentText.text = content;

            PlayOpenAnimation(animationDuration, onAnimationFinished);
        }

        public void HideNotification(float animationDuration, System.Action onAnimationFinished)
        {
            PlayCloseAnimation(animationDuration, onAnimationFinished);
        }

        public void Update()
        {
            if (Input.GetKeyUp(KeyCode.O))
            {
                ShowNotification("Test Title", "Test Content", Color.red, .5f, null);
            }
            if (Input.GetKeyUp(KeyCode.C))
            {
                HideNotification(.5f, null);
            }
        }




        private void PlayOpenAnimation(float duration, System.Action onAnimationFinished)
        {
            // Stop any previous animation
            if (lowerAnim != null)
                LeanTween.cancel(lowerAnim.id);
            if (upperAnim != null)
                LeanTween.cancel(upperAnim.id);
            if (fadeAnim != null)
                LeanTween.cancel(fadeAnim.id);

            // Play animation
            lowerAnim = LeanTween.value(lowerPanel, ((RectTransform)lowerPanel.transform).anchoredPosition.y, 0, duration).setEaseOutQuart().setOnComplete(onAnimationFinished).setOnUpdate((float value) =>
            {
                Vector3 pos = ((RectTransform)lowerPanel.transform).anchoredPosition;
                pos.y = value;
                ((RectTransform)lowerPanel.transform).anchoredPosition = pos;
            });
            upperAnim = LeanTween.value(upperPanel, ((RectTransform)upperPanel.transform).anchoredPosition.y, 0, duration).setEaseOutQuart().setOnUpdate((float value) =>
            {
                Vector3 pos = ((RectTransform)upperPanel.transform).anchoredPosition;
                pos.y = value;
                ((RectTransform)upperPanel.transform).anchoredPosition = pos;
            });

            fadeAnim = LeanTween.value(fadePanel.gameObject, fadePanel.color.a, .8f, duration).setEaseOutQuart().setOnUpdate((float val) =>
            {
                Color c = fadePanel.color;
                c.a = val;
                fadePanel.color = c;
            });
        }

        private void PlayCloseAnimation(float duration, System.Action onAnimationFinished)
        {
            // Stop any previous animation
            if (lowerAnim != null)
                LeanTween.cancel(lowerAnim.id);
            if (upperAnim != null)
                LeanTween.cancel(upperAnim.id);
            if (fadeAnim != null)
                LeanTween.cancel(fadeAnim.id);

            // Play animation
            lowerAnim = LeanTween.value(lowerPanel, ((RectTransform)lowerPanel.transform).anchoredPosition.y, -notificationBarHeight, duration).setEaseOutQuart().setOnComplete(onAnimationFinished).setOnUpdate((float value)=>
            {
                Vector3 pos = ((RectTransform)lowerPanel.transform).anchoredPosition;
                pos.y = value;
                ((RectTransform)lowerPanel.transform).anchoredPosition = pos;
            });
            upperAnim = LeanTween.value(upperPanel, ((RectTransform)upperPanel.transform).anchoredPosition.y, notificationBarHeight, duration).setEaseOutQuart().setOnUpdate((float value) =>
            {
                Vector3 pos = ((RectTransform)upperPanel.transform).anchoredPosition;
                pos.y = value;
                ((RectTransform)upperPanel.transform).anchoredPosition = pos;
            });
            fadeAnim = LeanTween.value(fadePanel.gameObject, fadePanel.color.a, 0f, duration).setEaseOutQuart().setOnUpdate((float val) =>
            {
                Color c = fadePanel.color;
                c.a = val;
                fadePanel.color = c;
            });

        }
    }
}
