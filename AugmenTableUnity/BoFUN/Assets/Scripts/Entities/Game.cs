using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BoFUN.Entities
{
    [System.Serializable]
    public class Game
    {
        public int duration;
        public int totalPlayers;
        public Player[] players;
        public Team[] teams;
        public bool pantomime;
        public bool pictionary;
        public bool trivia;
        public int[] sequence;  // The team play sequence
        public int winningTeam;
        public Round[] rounds;


        /// <summary>
        /// Creates a Game Instance from a JSON string
        /// </summary>
        /// <param name="jsonString"></param>
        /// <returns></returns>
        public static Game CreateFromJSON(string jsonString)
        {
            return JsonUtility.FromJson<Game>(jsonString);
        }

        public override string ToString()
        {
            return JsonUtility.ToJson(this);
        }
    }
}