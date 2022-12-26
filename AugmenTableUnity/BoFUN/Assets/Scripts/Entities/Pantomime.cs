using UnityEngine;

namespace BoFUN.Entities
{
    class Pantomime
    {
        public int id = -1;
        public string category = "";
        public string task = "";

        public static string ToJSON(Pantomime p)
        {
            return JsonUtility.ToJson(p);
        }

        public static Pantomime CreateFromJSON(string jsonString)
        {
            return JsonUtility.FromJson<Pantomime>(jsonString);
        }
    }
}
