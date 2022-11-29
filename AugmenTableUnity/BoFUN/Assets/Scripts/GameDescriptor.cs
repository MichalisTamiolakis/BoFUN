using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BoFUN.GameManager
{
    public class GameDescriptor
    {
        public int numberOfPlayers = 4;
        public int numberOfTeams = 2;
        public bool trivia = true;
        public bool pantomime = true;
        public bool pictionary = true;
        public int timePerRound = 120; // In seconds

        public override string ToString()
        {
            TimeSpan t = TimePerRoundToTimeSpan();

            return $"Number of players: {numberOfPlayers}\n" +
                   $"Number of teams: {numberOfTeams}\n" +
                   $"Games: " + (trivia ? "Trivia " : "") + (pantomime ? "Pantomime " : "") + (pictionary ? "Pictionary " : "") + "\n" +
                   $"Time per Round: {t.Minutes:00} : {t.Seconds:00}";
        }

        public TimeSpan TimePerRoundToTimeSpan()
        {
            TimeSpan t = new TimeSpan(0, 0, timePerRound);
            return t;
        }
    }
}