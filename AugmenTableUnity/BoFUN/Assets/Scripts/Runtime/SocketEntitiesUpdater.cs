using BoFUN.Entities;
using BoFUN.GameManager;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// Updates Teams/Rounds/Players at runtime using web socket events
public class SocketEntitiesUpdater : MonoBehaviour
{
    public static SocketEntitiesUpdater Instance
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


    // Start is called before the first frame update
    void Start()
    {
        SocketEventHandler.Instance.teamUpdatedEvent.AddListener(HandleTeamUpdateEvent);
        SocketEventHandler.Instance.playerUpdatedEvent.AddListener(HandlePlayerUpdateEvent);
        SocketEventHandler.Instance.roundUpdatedEvent.AddListener(HandleRoundUpdateEvent);

    }

    private void HandleTeamUpdateEvent(Team t)
    {
        Team oldTeamData = GameManager.Instance.currentGame.GetTeam(t.id);
        if (oldTeamData!=null)
        {
            oldTeamData.UpdateFrom(t);
            //Debug.Log("Inco")
            oldTeamData.onUpdate.Invoke(oldTeamData);
        }
        else
        {
            Array.Resize(ref GameManager.Instance.currentGame.teams, GameManager.Instance.currentGame.teams.Length + 1);
            GameManager.Instance.currentGame.teams[GameManager.Instance.currentGame.teams.Length - 1] = t;
        }
    }

    private void HandlePlayerUpdateEvent(Player p)
    {
        Player oldPlayerData = GameManager.Instance.currentGame.GetPlayer(p.id);
        if (oldPlayerData != null)
        {
            oldPlayerData.UpdateFrom(p);
            //Debug.Log("Inco")
            oldPlayerData.onUpdate.Invoke(oldPlayerData);
        }
        else
        {
            Array.Resize(ref GameManager.Instance.currentGame.players, GameManager.Instance.currentGame.players.Length + 1);
            GameManager.Instance.currentGame.players[GameManager.Instance.currentGame.players.Length - 1] = p;
        }
    }

    private void HandleRoundUpdateEvent(Round r)
    {
        Round oldRound = GameManager.Instance.currentGame.GetRound(r.id);
        if (oldRound != null)
        {
            oldRound.UpdateFrom(r);
            //Debug.Log("Inco")
            oldRound.onUpdate.Invoke(oldRound);
        }
        else
        {
            Array.Resize(ref GameManager.Instance.currentGame.rounds, GameManager.Instance.currentGame.rounds.Length + 1);
            GameManager.Instance.currentGame.rounds[GameManager.Instance.currentGame.rounds.Length - 1] = r;
        }
    }
}
