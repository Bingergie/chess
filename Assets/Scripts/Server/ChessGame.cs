using System.Collections.Generic;
using System;
using Riptide;
using Server.ChessGameStates;
using Shared;
using UnityEngine;

namespace Server {
    public class ChessGame {
        public readonly int Width;
        public readonly int Height;
        public Piece[] Board { get; private set; }
        public bool IsWhiteTurn { get; private set; }
        public ushort[] PlayerIds { get; private set; }
        public ushort WhitePlayerId { get; private set; }
        public Stack<Move> MoveHistory { get; private set; }

        private ChessBaseState _currentState;
        private readonly Dictionary<Type, ChessBaseState> _stateDictionary;
        
        public ChessGame(ushort[] playerIds, int width = 8, int height = 8) {
            PlayerIds = playerIds;
            Width = width;
            Height = height;
            Board = new Piece[Width * Height];
            _stateDictionary = new Dictionary<Type, ChessBaseState> {
                { typeof(ChessStartState), new ChessStartState(this) },
                { typeof(ChessPlayingState), new ChessPlayingState(this) },
            };
            ChangeState(typeof(ChessStartState));
            
        }
        
        public void ChangeState(Type stateType) {
            _currentState = _stateDictionary[stateType];
            _currentState.Enter();
        }
        
        public bool IsWhite(int playerId) {
            return playerId == WhitePlayerId;
        }
        
        public void SetWhitePlayer(ushort playerId) {
            WhitePlayerId = playerId;
        }
        
        [MessageHandler((ushort)ClientToServerId.MakeMove)]
        private static void HandleMakeMove(ushort fromClientId, Message message) {
            var move = message.GetSerializable<Move>();
            var game = GameManager.Instance.GetGame(fromClientId);
            game.Move(move, game.IsWhite(fromClientId));
        }
        
        private void Move(Move move, bool isFromWhite) {
            _currentState.MakeMove(move, isFromWhite);
        }

        public void PushMove(Move move) {
            MoveHistory.Push(move);
        }

        public void SetBoard(Piece[] board) {
            Board = board;
        }

        public void SetIsWhiteTurn(bool isWhiteTurn) {
            IsWhiteTurn = isWhiteTurn;
        }
    }
}