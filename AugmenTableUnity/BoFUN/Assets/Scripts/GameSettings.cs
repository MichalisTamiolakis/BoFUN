using UnityEngine;

[System.Serializable]
public class GameSettings
{
    public int minNumberOfTeams = 2;
    public int maxNumberOfTeams = 4;
    public int minNumberOfPlayersPerTeam = 2;
    public int maxNumberOfPlayersPerTeam = 4;
    public int maxNumberOfPlayersTotal = 8;

    public static GameSettings CreateFromJSON(string jsonString)
    {
        return JsonUtility.FromJson<GameSettings>(jsonString);
    }
}
