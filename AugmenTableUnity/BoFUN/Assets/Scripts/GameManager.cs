using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BoFUN.GameManager
{
    public class GameManager : MonoBehaviour
    {
        public static GameManager Instance
        {
            get;
            private set;
        }

        private void Awake()
        {
            // If there is an instance, and it's not me, delete myself.

            if (Instance != null && Instance != this)
            {
                Destroy(this);
            }
            else
            {
                Instance = this;
                gameDescriptor = new GameDescriptor();
            }
        }

        public GameDescriptor gameDescriptor;

        void Start()
        {
        }

        void Update()
        {

        }
    }
}
