using System;
using UnityEngine;
using UnityEngine.Events;

namespace BoFUN.Entities
{
    [Serializable]
    public class Round
    {
        public int id;
        public int team;
        public int player;
        public MiniGame minigame;
        public string minigameJSON;
        public bool victory;
        public int remainingTime;
        public bool started;
        public bool ended;

        [NonSerialized]
        public UnityEvent<Round> onUpdate = new UnityEvent<Round>();

        public void UpdateFrom(Round r)
        {
            this.id = r.id;
            this.team = r.team;
            this.player = r.player;
            this.minigame = r.minigame;
            this.minigameJSON = r.minigameJSON;
            this.victory = r.victory;
            this.remainingTime = r.remainingTime;
            this.started = r.started;
            this.ended = r.ended;
        }

        /// <summary>
        /// Creates a Round Instance from a JSON string
        /// </summary>
        /// <param name="jsonString"></param>
        /// <returns></returns>
        public static Round CreateFromJSON(string jsonString)
        {
            return JsonUtility.FromJson<Round>(jsonString);
        }

        public static string ToJSON(Round r)
        {
            return JsonUtility.ToJson(r, true);
        }
    }

    public enum MiniGame
    {
        Pantomime = 0,
        Trivia = 1,
        Pictionary = 2
    }
}
