using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace BoFUN.Entities
{
    [System.Serializable]
    public class Player
    {
        public int id;
        public string username;
        public int teamId;
        public string image;
        public int positionId;

        [NonSerialized]
        public UnityEvent<Player> onUpdate = new UnityEvent<Player>();


        /// <summary>
        /// Creates a Player Instance from a JSON string
        /// </summary>
        /// <param name="jsonString"></param>
        /// <returns></returns>
        public static Player CreateFromJSON(string jsonString)
        {
            return JsonUtility.FromJson<Player>(jsonString);
        }

        public static string ToJSON(Player p)
        {
            return JsonUtility.ToJson(p, true);
        }

        public void UpdateFrom(Player p)
        {
            this.id = p.id;
            this.username = p.username;
            this.teamId = p.teamId;
            this.image = p.image;
            this.positionId = p.positionId;
        }
    }
}
