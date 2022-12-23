using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace BoFUN.Entities
{
    [System.Serializable]
    public class Round
    {
        public int id;

        public UnityEvent<Round> onUpdate = new UnityEvent<Round>();

        /// <summary>
        /// Creates a Round Instance from a JSON string
        /// </summary>
        /// <param name="jsonString"></param>
        /// <returns></returns>
        public static Round CreateFromJSON(string jsonString)
        {
            return JsonUtility.FromJson<Round>(jsonString);
        }
    }
}
