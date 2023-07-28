using UnityEngine;

namespace Server {
    public class GameManager : Singleton<GameManager> {

        public void ClientConnected(int clientId) {
            Debug.Log("client connected");
        }

        public void ClientDisconnected(int clientId) {
            Debug.Log("client disconnected");
        }
    }
}