using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Crosstales.RTVoice;
using BoFUN.Utilities;
using UnityEngine.Networking;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance
    {
        get;
        private set;
    }

    public AudioSource audioSource;

    [Space(10)]
    public AudioClip boardAmbientSound;
    public AudioClip pictionaryAmbientSound;
    public AudioClip triviaAmbientSound;
    public AudioClip pantomimeAmbientSound;

    [Space(10)]
    public AudioClip correctAnswerSound;
    public AudioClip incorrectAnswerSound;
    public AudioClip timeFinishedSound;


    public static string[] audioPaths =
    {
        "/Sounds/BoardAmbient.mp3",
        "/Sounds/PictionaryAmbient.mp3",
        "/Sounds/TriviaAmbient.mp3",
        "/Sounds/PantomimeAmbient.mp3",

        "/Sounds/CorrectAnswer.mp3",
        "/Sounds/IncorrectAnswer.mp3",
        "/Sounds/TimeFinished.mp3",
    };

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

        // Load sounds from streaming assets
        StartCoroutine(ImportAudioClips());

    }

    private IEnumerator ImportAudioClips()
    {
        string url = "File://" + Application.streamingAssetsPath + audioPaths[0];

        using (UnityWebRequest www = UnityWebRequestMultimedia.GetAudioClip(url, AudioType.MPEG))
        {
            yield return www.SendWebRequest();

            if (www.result == UnityWebRequest.Result.ConnectionError)
            {
                Debug.Log(www.error);
            }
            else
            {
                boardAmbientSound = DownloadHandlerAudioClip.GetContent(www);
            }

            www.Dispose();
        }

        url = "File://" + Application.streamingAssetsPath + audioPaths[1];

        using (UnityWebRequest www = UnityWebRequestMultimedia.GetAudioClip(url, AudioType.MPEG))
        {
            yield return www.SendWebRequest();

            if (www.result == UnityWebRequest.Result.ConnectionError)
            {
                Debug.Log(www.error);
            }
            else
            {
                pictionaryAmbientSound = DownloadHandlerAudioClip.GetContent(www);
            }

            www.Dispose();
        }

        url = "File://" + Application.streamingAssetsPath + audioPaths[2];

        using (UnityWebRequest www = UnityWebRequestMultimedia.GetAudioClip(url, AudioType.MPEG))
        {
            yield return www.SendWebRequest();

            if (www.result == UnityWebRequest.Result.ConnectionError)
            {
                Debug.Log(www.error);
            }
            else
            {
                triviaAmbientSound = DownloadHandlerAudioClip.GetContent(www);
            }

            www.Dispose();

        }

        url = "File://" + Application.streamingAssetsPath + audioPaths[3];
        using (UnityWebRequest www = UnityWebRequestMultimedia.GetAudioClip(url, AudioType.MPEG))
        {
            yield return www.SendWebRequest();

            if (www.result == UnityWebRequest.Result.ConnectionError)
            {
                Debug.Log(www.error);
            }
            else
            {
                pantomimeAmbientSound = DownloadHandlerAudioClip.GetContent(www);
            }

            www.Dispose();

        }

        url = "File://" + Application.streamingAssetsPath + audioPaths[4];
        using (UnityWebRequest www = UnityWebRequestMultimedia.GetAudioClip(url, AudioType.MPEG))
        {
            yield return www.SendWebRequest();

            if (www.result == UnityWebRequest.Result.ConnectionError)
            {
                Debug.Log(www.error);
            }
            else
            {
                correctAnswerSound = DownloadHandlerAudioClip.GetContent(www);
            }

            www.Dispose();

        }

        url = "File://" + Application.streamingAssetsPath + audioPaths[5];
        using (UnityWebRequest www = UnityWebRequestMultimedia.GetAudioClip(url, AudioType.MPEG))
        {
            yield return www.SendWebRequest();

            if (www.result == UnityWebRequest.Result.ConnectionError)
            {
                Debug.Log(www.error);
            }
            else
            {
                incorrectAnswerSound = DownloadHandlerAudioClip.GetContent(www);
            }

            www.Dispose();

        }

        url = "File://" + Application.streamingAssetsPath + audioPaths[6];
        using (UnityWebRequest www = UnityWebRequestMultimedia.GetAudioClip(url, AudioType.MPEG))
        {
            yield return www.SendWebRequest();

            if (www.result == UnityWebRequest.Result.ConnectionError)
            {
                Debug.Log(www.error);
            }
            else
            {
                timeFinishedSound = DownloadHandlerAudioClip.GetContent(www);
            }

            www.Dispose();

        }
    }


    public void Start()
    {
        Speaker.Instance.OnSpeakStarted.AddListener(OnSpeechCompleted);
        Speaker.Instance.OnSpeakCompleted.AddListener(OnSpeechStarted);

        audioSource.loop = true;
        audioSource.clip = null;
    }


    public void PlayBoardSound() 
    {
        StopAllSounds();

        audioSource.Stop();
        audioSource.clip = boardAmbientSound;
        audioSource.Play();
    }

    public void PlayTriviaSound()
    {
        StopAllSounds();

        audioSource.Stop();
        audioSource.clip = triviaAmbientSound;
        audioSource.Play();
    }

    public void PlayPantomimeSound()
    {
        StopAllSounds();

        audioSource.Stop();
        audioSource.clip = pantomimeAmbientSound;
        audioSource.Play();
    }

    public void PlayPictionarySound()
    {
        StopAllSounds();

        audioSource.Stop();
        audioSource.clip = pictionaryAmbientSound;
        audioSource.Play();
    }

    public void PlayCorrectAnswerSound()
    {
        audioSource.PlayOneShot(correctAnswerSound, 0.7f);
    }

    public void PlayIncorrectAnswerSound()
    {
        audioSource.PlayOneShot(incorrectAnswerSound, 0.7f);

    }

    public void PlayTimeFinishedSound()
    {
        audioSource.PlayOneShot(timeFinishedSound, 0.7f);
    }

    public void StopAllSounds()
    {
        audioSource.Stop();
        Speaker.Instance.Silence();
    }

    public void Speech(string text)
    {
        Speaker.Instance.SpeakNative(text);
    }

    public void OnSpeechStarted(string text)
    {
        audioSource.Pause();
    }

    public void OnSpeechCompleted(string text)
    {
        audioSource.UnPause();
    }

}
