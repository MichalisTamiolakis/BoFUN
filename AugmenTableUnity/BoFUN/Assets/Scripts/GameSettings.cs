using UnityEngine;

[System.Serializable]
public class GameSettings
{
    public int minNumberOfTeams = 2;
    public int maxNumberOfTeams = 4;
    public int minNumberOfPlayersPerTeam = 2;
    public int maxNumberOfPlayersPerTeam = 4;
    public int maxNumberOfPlayersTotal = 8;

    [Space(10)]
    public int minRoundDurationSeconds = 30;
    public int maxRoundDurationSeconds = 300;

    [Space(10)]
    public bool confettiOnCorrectAnswer = true;
    public bool confettinOnGameFinished = true;

    [Space(10)]
    public bool naratorEnabled = false;

    public static GameSettings CreateFromJSON(string jsonString)
    {
        return JsonUtility.FromJson<GameSettings>(jsonString);
    }
}
