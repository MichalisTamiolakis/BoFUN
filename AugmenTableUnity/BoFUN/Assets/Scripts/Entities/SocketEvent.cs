using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SocketIOClient;

[System.Serializable]
public class SocketEvent
{
    SocketIOResponse socketEventResponse;

    public string GetEventName()
    {
        return socketEventResponse.GetValue<string>(0);
    }

    public T GetData<T>()
    {
        return socketEventResponse.GetValue<T>(1);
    }

    public SocketEvent(SocketIOResponse response)
    {
        this.socketEventResponse = response;
    }

}
