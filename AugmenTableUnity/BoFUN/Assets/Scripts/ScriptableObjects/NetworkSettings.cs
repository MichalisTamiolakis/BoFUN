using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NetworkSettings", menuName = "BoFUN/NetworkSettings", order = 1)]
public class NetworkSettings : ScriptableObject
{
    public string serverURL = "localhost:8080/BoFUN";
    public string frontendURL = "localhost:4200";
    
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
        public string assignPlayerToTeamPath;
        public string setPlayerNamePath;

        // Delete requests
        public string deletePlayerFromTeamPath;

        public string GetTeamPath(int teamId)
        {
            return getTeamPath.Replace("$teamId", teamId.ToString());
        }

        public string GetAssignPlayerToTeamPath(int playerId, int teamId)
        {
            return assignPlayerToTeamPath.Replace("$playerId", playerId.ToString()).Replace("$teamId", teamId.ToString());
        }

        public string GetSetPlayerNamePath(int playerId)
        {
            return setPlayerNamePath.Replace("$playerId", playerId.ToString());
        }

        public string GetDeletePlayerFromTeamPath(int playerId, int teamId)
        {
            return deletePlayerFromTeamPath.Replace("$playerId", playerId.ToString()).Replace("$teamId", teamId.ToString());
        }
    }
    [Space(5)]
    [Header("Game API:")]
    public GameAPI gameAPI = new GameAPI { 
        getGamePath = "game",
        getAllTeamsPath= "teams",
        getTeamPath="/team/$teamId",
        createGamePath = "game/create",
        createPlayerPath="game/createPlayer",
        assignPlayerToTeamPath= "assignPlayerToTeam/$playerId",
        setPlayerNamePath= "setPlayerName/$playerId",
        deletePlayerFromTeamPath= "removePlayer/$playerId/fromTeam/$teamId"
    };

    [System.Serializable]
    public struct Sockets {
        public string socketServerURL;
    }
    [Space(5)]
    public Sockets sockets = new Sockets {
        socketServerURL = "ws:localhost:8080"
    };

    [System.Serializable]
    public struct FrontEnd
    {
        public string joinPagePath;

        public string GetJoinPagePath(int seatId)
        {
            return joinPagePath.Replace("$seatId", seatId.ToString());
        }
    }
    [Space(5)]
    public FrontEnd frontEnd = new FrontEnd{ joinPagePath = "join/$seatId" };


}
