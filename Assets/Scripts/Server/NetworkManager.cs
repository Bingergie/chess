using Riptide.Utils;
using Riptide;
using UnityEngine;

namespace Server {
    public class NetworkManager : Singleton<NetworkManager> {

        public Riptide.Server Server { get; private set; }

        [SerializeField] private ushort port;
        [SerializeField] private ushort maxClientCount;

        private void Start() {
#if UNITY_EDITOR
            RiptideLogger.Initialize(Debug.Log, Debug.Log, Debug.LogWarning, Debug.LogError, false);
#else
        System.Console.Title = "Server";
        System.Console.Clear();
        Application.SetStackTraceLogType(UnityEngine.LogType.Log, StackTraceLogType.None);
        RiptideLogger.Initialize(Debug.Log, true);
#endif
            Server = new Riptide.Server();
            Server.ClientConnected += OnClientConnected;
            Server.ClientDisconnected += OnClientDisconnected;
            Server.Start(port, maxClientCount);
        }

        private void FixedUpdate() {
            Server.Update();
        }

        private void OnClientConnected(object sender, ServerConnectedEventArgs e) {
            Debug.Log($"Client {e.Client.Id} connected!");
            GameManager.Instance.ClientConnected(e.Client.Id);
        }
        
        private void OnClientDisconnected(object sender, ServerDisconnectedEventArgs e) {
            Debug.Log($"Client {e.Client.Id} disconnected!");
            GameManager.Instance.ClientDisconnected(e.Client.Id);
        }
    }
}