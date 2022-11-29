using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NetworkData", menuName = "BoFUN/NetworkSettings", order = 1)]
public class NetworkSettings : ScriptableObject
{
    public string serverURI = "localhost:4200";
    public string createGamePath = "newGame";
}
