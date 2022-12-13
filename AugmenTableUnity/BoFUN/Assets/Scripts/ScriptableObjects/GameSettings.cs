using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "GameSettings", menuName = "BoFUN/GameSettings", order = 1)]
public class GameSettings : ScriptableObject
{
    public int minNumberOfTeams = 2;
    public int maxNumberOfTeams = 4;
    public int minNumberOfPlayersPerTeam = 2;
    public int maxNumberOfPlayersPerTeam = 4;
    public int maxNumberOfPlayersTotal = 8;
}
