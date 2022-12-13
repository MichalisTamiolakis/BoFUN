using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BoFUN.Entities
{
    [System.Serializable]
    public class Round
    {
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
