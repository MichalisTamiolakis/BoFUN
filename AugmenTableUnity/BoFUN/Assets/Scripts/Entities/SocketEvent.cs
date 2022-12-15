using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SocketEvent : MonoBehaviour
{
    public string eventName;
    public string data;

    public SocketEvent(string eventName, string data)
    {
        this.eventName = eventName;
        this.data = data;
    }

    public void SetData(string data)
    {
        this.data = data;
    }

    public void SetData(object data)
    {
        this.data = JsonUtility.ToJson(data);
    }

    public static SocketEvent FromJsonString(string jsonString)
    {
        return JsonUtility.FromJson<SocketEvent>(jsonString);
    }

    public string toJsonString(bool prettyPrint=false)
    {
        return JsonUtility.ToJson(this, prettyPrint);
    }
}
