using System;
using UnityEngine;

namespace Client {
    public class GameManager : Singleton<GameManager> {
        public void Connected(int clientId) {
            Debug.Log($"client connected as {clientId}");
        }
    }
}