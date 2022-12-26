using UnityEngine;

namespace BoFUN.Entities
{
    class Pictionary
    {
        public int id=-1;
        public int difficulty = 0;
        public string task = "";

        public static string ToJSON(Pictionary p)
        {
            return JsonUtility.ToJson(p);
        }

        public static Pictionary CreateFromJSON(string jsonString)
        {
            return JsonUtility.FromJson<Pictionary>(jsonString);
        }
    }
}
