using System;
using Riptide;
using Shared;
using UnityEngine;

namespace Client {
    public class GameManager : Singleton<GameManager> {
        public event EventHandler<Piece[]> OnGameStart;
        public event EventHandler<Move> OnBoardUpdated;
        public static bool IsWhite { get; private set; }
        public void Connected(int clientId) {
            Debug.Log($"client connected as {clientId}");
        }

        public void TryMakeMove(Move move) {
            Message message = Message.Create(MessageSendMode.Reliable, (ushort)ClientToServerId.MakeMove);
            message.Add(move);
            NetworkManager.Instance.Client.Send(message);
        }
        
        [MessageHandler((ushort)ServerToClientId.PairingSuccess)]
        private static void HandlePairingSuccess(Message message) {
            Debug.Log("pairing success");
        }
        
        [MessageHandler((ushort)ServerToClientId.GameStart)]
        private static void HandleGameStart(Message message) {
            IsWhite = message.GetUShort() == NetworkManager.Instance.Client.Id;
            Debug.Log("game start");
            Debug.Log("you are " + (IsWhite ? "white" : "black"));
            var pieces = message.GetSerializables<Piece>();
            Instance.OnGameStart?.Invoke(Instance, pieces);
        }
        
        [MessageHandler((ushort)ServerToClientId.MoveMade)]
        private static void HandleMoveMade(Message message) {
            var move = message.GetSerializable<Move>();
            Debug.Log("move made");
            Instance.OnBoardUpdated?.Invoke(Instance, move);
        }
    }
}