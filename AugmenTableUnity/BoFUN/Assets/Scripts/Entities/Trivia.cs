using System.Collections.Generic;
using UnityEngine;

namespace BoFUN.Entities
{
    [System.Serializable]
    class Trivia
    {
        public int id;
        public string category = "";
        public string question = "";
        public string[] answers;
        public int correctAnswer = -1;

        public static string ToJSON(Trivia t)
        {
            return JsonUtility.ToJson(t);
        }

        public static Trivia CreateFromJSON(string jsonString)
        {
            return JsonUtility.FromJson<Trivia>(jsonString);
        }
    }
}
