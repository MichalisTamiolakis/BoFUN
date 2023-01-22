using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class NotificationManager : MonoBehaviour
{
    public static NotificationManager Instance
    {
        get;
        private set;
    }

    private void Awake()
    {
        // If there is an instance, and it's not me, delete myself.

        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }
    }

    public GameObject notificationCamera;
    public GameObject notificationScreen;
    public GameObject sphere;
    public Image sphereImage;
    public List<TMP_Text> sphereTexts;
    public List<ParticleSystem> particles;
    public Color correctColor;
    public Color errorColor;


    private LTDescr animationDescriptor;

    public void ShowCorrectNotificationSphere(string text, float time, Action onComplete, float startDelay = 0, float animationsTime = .5f)
    {
        ShowNotificationSphere(text, correctColor, true, time, onComplete, startDelay, animationsTime);
    }

    public void ShowErrorNotificationSphere(string text, float time, Action onComplete, float startDelay = 0, float animationsTime = .5f)
    {
        ShowNotificationSphere(text, errorColor, false, time, onComplete, startDelay, animationsTime);
    }

    public void ShowNotificationSphere(string text, Color bgColor, bool confetti, float time, Action onComplete, float startDelay=0, float animationsTime=.5f)
    {
        CancelNotification();
        sphere.transform.localScale = Vector3.zero;
        sphereImage.color = bgColor;
        foreach (var sText in sphereTexts)
        {
            sText.text = text;
        }

        notificationCamera.SetActive(true);
        notificationScreen.SetActive(true);
        
        
        animationDescriptor = LeanTween.scale(sphere, Vector3.one, animationsTime).setDelay(startDelay).setEaseOutBack().setOnStart(()=> {
            if(confetti)
                foreach (ParticleSystem ps in particles)
                {
                    ps.Play();
                }
        }).setOnComplete(()=>
        {
            animationDescriptor = LeanTween.delayedCall(time, () => {

                animationDescriptor = LeanTween.scale(sphere, Vector3.zero, animationsTime).setEaseInQuart().setOnComplete(() =>
                {
                    notificationScreen.SetActive(false);
                    notificationCamera.SetActive(false);

                    onComplete?.Invoke();
                });
            });
        });
    }


    public void CancelNotification()
    {   
        if(animationDescriptor!=null)
            LeanTween.cancel(animationDescriptor.id);

        notificationCamera.SetActive(false);
        notificationScreen.SetActive(false);
        foreach(ParticleSystem ps in particles)
        {
            ps.Stop();
        }
    }
}
