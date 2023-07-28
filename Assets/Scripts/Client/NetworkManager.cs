using System;
using Riptide;
using Riptide.Utils;
using UnityEngine;

namespace Client {
    public class NetworkManager : Singleton<NetworkManager> {

        public Riptide.Client Client { get; private set; }
        
        [SerializeField] private string ip;
        [SerializeField] private ushort port;

        private void Start() {
#if UNITY_EDITOR
            RiptideLogger.Initialize(Debug.Log, Debug.Log, Debug.LogWarning, Debug.LogError, false);
#else
        System.Console.Title = "Client";
        System.Console.Clear();
        Application.SetStackTraceLogType(UnityEngine.LogType.Log, StackTraceLogType.None);
        RiptideLogger.Initialize(Debug.Log, true);
#endif
            Client = new Riptide.Client();
            Client.Connected += OnConnected;
            Client.ConnectionFailed += OnConnectionFailed;
            Client.Disconnected += OnDisconnected;
            
            Connect();
        }

        private void Update() {
            Client.Update();
        }

        private void OnApplicationQuit() {
            Client.Disconnect();
        }

        public void Connect() {
            Client.Connect($"{ip}:{port}");
        }

        private void OnConnected(object sender, EventArgs e) {
            GameManager.Instance.Connected(Client.Id);
        }
        
        private void OnConnectionFailed(object sender, ConnectionFailedEventArgs e) {
            
        }

        private void OnDisconnected(object sender, DisconnectedEventArgs e) {
            
        }
    }
}