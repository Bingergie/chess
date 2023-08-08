using System;
using System.Collections.Generic;
using System.Linq;
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
        
        [MessageHandler((ushort)ClientToServerId.MakeMove)]
        private static void HandleMakeMove(ushort fromClientId, Message message) {
            var move = message.GetSerializable<Move>();
            var game = GameManager.Instance.GetGame(fromClientId);
            game.MakeMove(move, game.IsWhite(fromClientId));
        }

        #endregion

        private ChessGame GetGame(ushort fromClientId) {
            return _games.FirstOrDefault(game => game.PlayerIds[0] == fromClientId || game.PlayerIds[1] == fromClientId);
        }
    }
}