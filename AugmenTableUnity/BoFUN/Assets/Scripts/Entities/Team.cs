using UnityEngine;

namespace BoFUN.Entities
{
    [System.Serializable]
    public class Team
    {
        public int id;
        public string name;
        public string image;
        public int[] members;
        public string color;


        /// <summary>
        /// Creates a Team Instance from a JSON string
        /// </summary>
        /// <param name="jsonString"></param>
        /// <returns></returns>
        public static Team CreateFromJSON(string jsonString)
        {
            return JsonUtility.FromJson<Team>(jsonString);
        }

        /// <summary>
        /// Updates data, by copying them from the given Team t
        /// </summary>
        /// <param name="t"></param>
        public void UpdateFrom(Team t)
        {
            id = t.id;
            name = t.name;
            image = t.image;
            members = t.members;
            color = t.color;
        }
    }
}
