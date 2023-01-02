using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BoFUN.GameManager
{
    [System.Serializable]
    public class GameCreationDescriptor
    {
        public int totalPlayers = 4;
        public int minPlayersPerTeam = 2;
        public int maxPlayersPerTeam = 4;
        public int totalTeams = 2;
        public bool trivia = true;
        public bool pantomime = true;
        public bool pictionary = true;
        public int duration = 120; // In seconds

        public override string ToString()
        {
            TimeSpan t = TimePerRoundToTimeSpan();

            return $"Number of players: {totalPlayers}\n" +
                   $"Number of teams: {totalTeams}\n" +
                   $"Games: " + (trivia ? "Trivia " : "") + (pantomime ? "Pantomime " : "") + (pictionary ? "Pictionary " : "") + "\n" +
                   $"Time per Round: {t.Minutes:00} : {t.Seconds:00}";
        }

        public string toJSON()
        {
            return JsonUtility.ToJson(this);
        }

        public TimeSpan TimePerRoundToTimeSpan()
        {
            TimeSpan t = new TimeSpan(0, 0, duration);
            return t;
        }
    }
}