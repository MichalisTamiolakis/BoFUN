using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using BoFUN.Utilities;
using BoFUN.GameManager;
using BoFUN.Entities;

public class SocketEventHandler : MonoBehaviour
{
    public static SocketEventHandler Instance
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


    [Space(10)]
    public UnityEvent<int> seatOccupiedEvent = new UnityEvent<int>();
    public UnityEvent<Team> teamUpdatedEvent = new UnityEvent<Team>();
    public UnityEvent<Player> playerUpdatedEvent = new UnityEvent<Player>();
    public UnityEvent<Round> roundUpdatedEvent = new UnityEvent<Round>();

    private void Start()
    {

        // Seat Occupied
        NetworkUtilities.Instance.SocketSubscribe(GameManager.Instance.networkSettings.sockets.seatOccupiedEvent, (SocketEvent e) => {
            int seatNo = e.GetData<int>();
            Debug.Log("SocketEvent: SeatOccupied:" + seatNo);

            seatOccupiedEvent.Invoke(seatNo);
        });

        // Team Updated
        NetworkUtilities.Instance.SocketSubscribe(GameManager.Instance.networkSettings.sockets.teamUpdatedEvent, (SocketEvent e) => {
            string jsonString = e.GetData<string>();

            Debug.Log("SocketEvent: TeamUpdated:" + jsonString);

            Team newTeamData = Team.CreateFromJSON(jsonString);

            teamUpdatedEvent.Invoke(newTeamData);
        });

        // Player Updated
        NetworkUtilities.Instance.SocketSubscribe(GameManager.Instance.networkSettings.sockets.playerUpdatedEvent, (SocketEvent e) =>
        {
            string jsonString = e.GetData<string>();

            Debug.Log("SocketEvent: PlayerUpdated:" + jsonString);
            
            Player newPlayerData = Player.CreateFromJSON(jsonString);


            playerUpdatedEvent.Invoke(newPlayerData);

        });

        // Round Updated
        NetworkUtilities.Instance.SocketSubscribe(GameManager.Instance.networkSettings.sockets.roundUpdatedEvent, (SocketEvent e) => {
            string jsonString = e.GetData<string>();

            Debug.Log("SocketEvent: RoundUpdated:" + jsonString);

            Round newRoundData = Round.CreateFromJSON(jsonString);

            roundUpdatedEvent.Invoke(newRoundData);
        });
    }

}
