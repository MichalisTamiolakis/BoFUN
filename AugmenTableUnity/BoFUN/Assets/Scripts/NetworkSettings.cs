using BoFUN.Entities;
using UnityEngine;

[System.Serializable]
public class NetworkSettings
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
        public string getNextTeamPath;

        // Post reqeusts
        public string createGamePath;
        public string startGamePath;
        public string endGamePath;
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
    
    [System.Serializable]
    public struct RoundAPI
    {
        // Post reqeusts
        public string newRoundPath;

        public string GetNewRoundPath(MiniGame miniGame)
        {
            return newRoundPath.Replace("$miniGame", ((int)miniGame).ToString());
        }
    }

    [Space(5)]
    [Header("Game API:")]
    public GameAPI gameAPI = new GameAPI { 
        getGamePath = "game",
        getAllTeamsPath= "game/teams",
        getTeamPath="game/team/$teamId",
        createGamePath = "game/create",
        startGamePath = "game/start",
        endGamePath = "game/end",
        createPlayerPath="game/createPlayer",
        assignPlayerToTeamPath= "game/assignPlayerToTeam/$playerId",
        setPlayerNamePath= "game/setPlayerName/$playerId",
        deletePlayerFromTeamPath= "game/removePlayer/$playerId/fromTeam/$teamId",
        getNextTeamPath = "game/nextTeam"
    };

    [Header("Round API:")]
    public RoundAPI roundAPI = new RoundAPI
    {
        newRoundPath = "game/round/new/$miniGame"
    };

    [System.Serializable]
    public struct Sockets {
        public string socketServerURL;
        [Space(5)]
        public string seatOccupiedEvent;
        public string teamUpdatedEvent;
        public string playerUpdatedEvent;
        public string roundUpdatedEvent;
        public string pictionaryDrawingUpdatedEvent;
    }
    [Space(5)]
    public Sockets sockets = new Sockets {
        socketServerURL = "http://localhost:8080",
        seatOccupiedEvent = "SeatOccupied",
        teamUpdatedEvent = "TeamUpdated",
        playerUpdatedEvent = "PlayerUpdated",
        roundUpdatedEvent = "RoundUpdated",
        pictionaryDrawingUpdatedEvent = "PictionaryDrawingUpdated"
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

    public static NetworkSettings CreateFromJSON(string jsonString)
    {
        return JsonUtility.FromJson<NetworkSettings>(jsonString);
    }
}
