using System;
using UnityEngine;

using NativeWebSocket;

public class WebsocketManager : MonoBehaviour
{
    [NonSerialized]
    public WebsocketManager Instance;

    private WebSocket _socket;
    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        _socket = new WebSocket("ws://192.168.0.60");
        _socket.OnOpen += () =>
        {
            Debug.Log("Connection opened!");
        };

        _socket.OnMessage += (data) =>
        {
            
        };
    }
}
