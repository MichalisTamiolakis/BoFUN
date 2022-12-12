using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using BoFUN.GameManager;

public class TeamAssignmentManager : MonoBehaviour
{
    public static TeamAssignmentManager Instance
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


    public GameObject TeamJoinScreen;
    public GameObject content;
    public GameObject loading;

    [Header("Prefabs")]
    public GameObject teamCardPrefab;

    public void StartUpdating()
    {
        EnableLoading(true);
        RemoveAllTeamCards();
    }

    public void StopUpdating()
    {

    }

    public void ShowScreen(bool show)
    {
        TeamJoinScreen.SetActive(show);
    }

    private void RemoveAllTeamCards()
    {
        foreach (Transform child in content.transform)
        {
            Destroy(child.gameObject);
        }
    }

    private void EnableLoading(bool enabled)
    {
        loading.SetActive(enabled);
    }

    private void FetchAllTeams()
    {
        UnityWebRequest request = UnityWebRequest.Post(GameManager.Instance.networkSettings.serverURL + "/" + GameManager.Instance.networkSettings.gameAPI.createGamePath, "");
        StartCoroutine(OnFetchAllTeamsRequestCompleted(request));
    }

    private IEnumerator OnFetchAllTeamsRequestCompleted(UnityWebRequest req)
    {
        yield return req.SendWebRequest();

        if (req.result != UnityWebRequest.Result.Success)
        {
            Debug.Log(req.error);
        }
        else
        {
            string jsonString = req.downloadHandler.text;
            Debug.Log(jsonString);
            // Parse result
            //req.result.t
        }
    }

}
