using System.Collections;
using UnityEngine;
using UnityEngine.Networking;
using WebSocketSharp;

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
        public WebSocket CreateSocket(string url, System.EventHandler<MessageEventArgs> onMessageReceived)
        {
            WebSocket ws = new WebSocket(url);
            ws.Connect();

            ws.OnMessage += onMessageReceived;

            return ws;
        }

        public void SubscribeToSocket(WebSocket ws, System.EventHandler<MessageEventArgs> onMessageReceived)
        {
            if(ws!=null)
                ws.OnMessage += onMessageReceived;
        }

        public void SendToSocket(WebSocket ws, string messgage)
        {
            if (ws!=null)
                ws.Send(messgage); 
        }

    }
}
