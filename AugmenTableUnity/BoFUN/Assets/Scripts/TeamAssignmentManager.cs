using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using BoFUN.GameManager;
using UnityEngine.UI;
using BoFUN.Entities;
using System;

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
    public Transform content;
    public GameObject loading;
    [Space(5)]
    public Button startGame;
    public Button exitGame;


    [Header("Prefabs")]
    public GameObject teamCardPrefab;

    public void ShowScreen(bool show)
    {
        TeamJoinScreen.SetActive(show);
    }


    public void StartUpdating()
    {
        EnableLoading(true);

        StartCoroutine("BeginRenderProcess");

        //SpawnTeamCards();
    }

    public void StopUpdating()
    {

    }

    private IEnumerator BeginRenderProcess()
    {
        yield return new WaitUntil(() => GameManager.Instance.game!=null && GameManager.Instance.game.teams!=null && GameManager.Instance.game.teams.Length>0);

        // Game has been loaded

        // Disable Loading screen
        EnableLoading(false);

        SpawnTeamCards();
    }


    // Private methods
    private List<TeamCard> spawnedTeamCards = new List<TeamCard>();
    private void SpawnTeamCards()
    {
        RemoveAllCards();

        // Sort teams by Id
        Array.Sort(GameManager.Instance.game.teams, (Team x, Team y) => x.id.CompareTo(y.id));

        // Add new team cards
        foreach (Team t in GameManager.Instance.game.teams)
        {
            GameObject teamCard = Instantiate(teamCardPrefab);
            teamCard.transform.SetParent(content.transform, false);

            TeamCard teamCardComponent;
            if(teamCard.TryGetComponent(out teamCardComponent))
            {
                teamCardComponent.associatedTeam = t;
                spawnedTeamCards.Add(teamCardComponent);
            }
        }

        // Render all cards
        foreach(TeamCard tc in spawnedTeamCards)
        {
            tc.Render();
        }
    }

    private void RemoveAllCards()
    {
        // Remove any old team cards
        foreach (Transform child in content.transform)
        {
            Destroy(child.gameObject);
        }
        spawnedTeamCards.Clear();
    }

    private void EnableLoading(bool enabled)
    {
        loading.SetActive(enabled);
    }
}
