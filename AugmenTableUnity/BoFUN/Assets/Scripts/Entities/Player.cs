using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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


        /// <summary>
        /// Creates a Player Instance from a JSON string
        /// </summary>
        /// <param name="jsonString"></param>
        /// <returns></returns>
        public static Player CreateFromJSON(string jsonString)
        {
            return JsonUtility.FromJson<Player>(jsonString);
        }
    }
}
