using System;
using System.Collections.Generic;
using Riptide;
using Shared;
using UnityEngine;

namespace Server {
    public class GameManager : Singleton<GameManager> {
        public static event EventHandler<int> OnClientConnected;
        public static event EventHandler<int> OnClientDisconnected;
        
        private List<ChessGame> _games;
        private List<ushort> _waitingPlayers;
        
        private void Start() {
            _games = new List<ChessGame>();
            _waitingPlayers = new List<ushort>();
        }

        public void ClientConnected(ushort clientId) {
            OnClientConnected?.Invoke(this, clientId);
            Debug.Log("client connected");
            
            if (_waitingPlayers.Count > 0) {
                var game = new ChessGame(new[] {clientId, _waitingPlayers[0]});
                _waitingPlayers.RemoveAt(0);
                _games.Add(game);
            } else {
                _waitingPlayers.Add(clientId);
            }
        }

        public void ClientDisconnected(ushort clientId) {
            OnClientDisconnected?.Invoke(this, clientId);
            Debug.Log("client disconnected");
        }

        #region Messages

        private void SendPairingSuccessMessage() {
            Message message = Message.Create(MessageSendMode.Reliable, (ushort)ServerToClientId.PairingSuccess);
            NetworkManager.Instance.Server.Send(message, _waitingPlayers[0]);
        }

        #endregion
    }
}