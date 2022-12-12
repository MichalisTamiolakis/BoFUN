using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NetworkData", menuName = "BoFUN/NetworkSettings", order = 1)]
public class NetworkSettings : ScriptableObject
{
    public string serverURL = "localhost:8080";
    
    [System.Serializable]
    public struct GameAPI
    {
        // Get Requests
        public string getGamePath;
        public string getAllTeamsPath;
        public string getTeamPath;

        // Post reqeusts
        public string createGamePath;
        public string createPlayerPath;

        // Put requests
        public string assignPlayerToTeam;
        public string setPlayerName;

        // Delete requests
        public string deletePlayerFromTeam;
    }
    [Space(5)]
    [Header("Game API:")]
    public GameAPI gameAPI = new GameAPI { getGamePath = "game", getAllTeamsPath= "teams", getTeamPath="/team/$teamId", createGamePath = "game/create", createPlayerPath="game/createPlayer", assignPlayerToTeam= "assignPlayerToTeam/:playerId", setPlayerName= "setPlayerName/:playerId", deletePlayerFromTeam= "removePlayer/:playerId/fromTeam/:teamId" };

    [Space(20)]
    public string socketServerURL = "ws:localhost:8080";


}
