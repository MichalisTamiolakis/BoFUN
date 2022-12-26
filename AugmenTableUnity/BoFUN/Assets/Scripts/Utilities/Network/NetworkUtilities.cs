using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Networking;
using SocketIOClient;
using SocketIOClient.Newtonsoft.Json;
using System.Collections.Generic;
using UnityEngine.Events;

namespace BoFUN.Utilities
{
    public class NetworkUtilities : MonoBehaviour
    {
        public static NetworkUtilities Instance
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
            }
        }

        // =========== HTTP Requests ===========

        /// <summary>
        /// Does a POST request in the given uri
        /// </summary>
        /// <param name="uri">The URI to perform the POST to</param>
        /// <param name="jsonString">The json string to send</param>
        /// <param name="onResponse">The callback to call when the request has finished.
        /// The bool passed will represent the response state and the string the response text.
        /// If the request has failed the string will contain the error</param>
        /// <returns>The web request made</returns>
        public UnityWebRequest Post(string uri, string jsonString, System.Action<bool, string> onResponse)
        {
            UnityWebRequest request = new UnityWebRequest(uri, "POST");
            request.SetRequestHeader("Content-Type", "application/json");
            byte[] jsonStringToBytes = new System.Text.UTF8Encoding().GetBytes(jsonString);
            request.uploadHandler = (UploadHandler)new UploadHandlerRaw(jsonStringToBytes);
            request.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();

            StartCoroutine(OnPostResponse(request, onResponse));

            return request;
        }

        private static IEnumerator OnPostResponse(UnityWebRequest req, System.Action<bool, string> onResponse)
        {
            yield return req.SendWebRequest();

            if (req.result != UnityWebRequest.Result.Success)
            {
                Debug.Log(req.error);
                onResponse(false, req.error);
            }
            else
            {
                string jsonResponse = req.downloadHandler.text;
                onResponse(true, jsonResponse);
            }

            req.Dispose();
        }

        /// <summary>
        /// Does a GET request in the given uri
        /// </summary>
        /// <param name="uri">The URI to perform the POST to</param>
        /// <param name="jsonString">The json string to send</param>
        /// <param name="onResponse">The callback to call when the request has finished.
        /// The bool passed will represent the response state and the string the response text.
        /// If the request has failed the string will contain the error</param>
        /// <returns>The web request made</returns>
        public UnityWebRequest Get(string uri, string jsonString, System.Action<bool, string> onResponse)
        {
            UnityWebRequest request = new UnityWebRequest(uri, "GET");
            request.SetRequestHeader("Content-Type", "application/json");
            byte[] jsonStringToBytes = new System.Text.UTF8Encoding().GetBytes(jsonString);
            request.uploadHandler = (UploadHandler)new UploadHandlerRaw(jsonStringToBytes);
            request.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();

            StartCoroutine(OnGetResponse(request, onResponse));

            return request;
        }

        private static IEnumerator OnGetResponse(UnityWebRequest req, System.Action<bool, string> onResponse)
        {
            yield return req.SendWebRequest();

            if (req.result != UnityWebRequest.Result.Success)
            {
                Debug.Log(req.error);
                onResponse(false, req.error);
            }
            else
            {
                string jsonResponse = req.downloadHandler.text;
                onResponse(true, jsonResponse);
            }

            req.Dispose();
        }

        // =========== Sockets ===========
        private SocketIOUnity socket;
        private Dictionary<string, UnityEvent<SocketEvent>> eventSubscriptions = new Dictionary<string, UnityEvent<SocketEvent>>();

        /// <summary>
        /// Connects to the websocket and starts listening to server events
        /// </summary>
        private async void ConnectToSocket()
        {
            var uri = new Uri(GameManager.GameManager.Instance.networkSettings.sockets.socketServerURL);
            socket = new SocketIOUnity(uri, new SocketIOOptions
            {
                Transport = SocketIOClient.Transport.TransportProtocol.WebSocket
            })
            {
                unityThreadScope = SocketIOUnity.UnityThreadScope.Update
            };

            await socket.ConnectAsync();

            Debug.Log($"Socket connected on {uri}");

            //SocketIOResponse
            socket.OnUnityThread("server:event", (response) =>
            {
                try
                {
                    SocketEvent ev = new SocketEvent(response);

                    if (eventSubscriptions.TryGetValue(ev.GetEventName(), out UnityEvent<SocketEvent> ue))
                    {
                        ue.Invoke(ev);
                    }
                }
                catch(Exception e)
                {
                    Debug.LogException(e);
                }

            });

        }


        /// <summary>
        /// Subscribe to a socket event
        /// </summary>
        /// <param name="eventName"></param>
        /// <param name="callback"></param>
        public void SocketSubscribe(string eventName, UnityAction<SocketEvent> callback)
        {
            if (eventSubscriptions.TryGetValue(eventName, out UnityEvent<SocketEvent> e))
            {
                e.AddListener(callback);
            }
            else
            {
                // Create new unity event
                UnityEvent<SocketEvent> newEvent = new UnityEvent<SocketEvent>();

                // Add it in dictionary
                eventSubscriptions[eventName] = newEvent;

                // Add listeners to UnityEvent
                newEvent.AddListener(callback);
            }
        }

        /// <summary>
        /// Unsubscribe from a socket event
        /// </summary>
        /// <param name="eventName"></param>
        /// <param name="callback"></param>
        public void SocketUnsubscribe(string eventName, UnityAction<SocketEvent> callback)
        {
            if(eventSubscriptions.TryGetValue(eventName, out UnityEvent<SocketEvent> e))
            {
                e.RemoveListener(callback);
            }
        }

        /// <summary>
        /// Publish to the socket
        /// </summary>
        /// <param name="eventName"></param>
        /// <param name="dataJsonString"></param>
        public void SocketPublish(string eventName, string dataJsonString)
        {
            if (socket.Connected)
            {
                socket.EmitStringAsJSON("client:event", "{\"{"+eventName+"}\": "+dataJsonString+"}");
            }
            else
            {
                Debug.LogError("Socket is not connected", this);
            }
            
        }

        public void Start()
        {
            ConnectToSocket();
        }

        public void OnDestroy()
        {
            if (socket!=null && socket.Connected)
            {
                socket.Disconnect();
            }
        }

    }
}
