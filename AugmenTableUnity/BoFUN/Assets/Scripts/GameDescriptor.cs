using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BoFUN.GameManager
{
    public class GameDescriptor : ScriptableObject
    {
        public int numberOfPlayers = 4;
        public int numberOfTeams = 2;
        public bool trivia = true;
        public bool pantomime = true;
        public bool pictionary = true;
        public int timePerRound = 120; // In seconds
    }
}