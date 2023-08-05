using System;
using Riptide;
using Shared;
using UnityEngine;

namespace Client {
    public class GameManager : Singleton<GameManager> {
        public void Connected(int clientId) {
            Debug.Log($"client connected as {clientId}");
        }
        
        [MessageHandler((ushort)ServerToClientId.PairingSuccess)]
        private static void HandlePairingSuccess(Message message) {
            Debug.Log("pairing success");
        }
        
        [MessageHandler((ushort)ServerToClientId.GameStart)]
        private static void HandleGameStart(Message message) {
            var isWhite = message.GetUShort() == NetworkManager.Instance.Client.Id;
            Debug.Log("game start");
            Debug.Log("you are " + (isWhite ? "white" : "black"));
        }
    }
}