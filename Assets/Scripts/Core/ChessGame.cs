using System;
using System.Collections.Generic;
using UnityEngine;

namespace Core {
    public class ChessGame {
        public event EventHandler<Piece[]> OnBoardUpdate;

        private Board _board;
        private ChessGameBaseState _currentState;
        private Dictionary<Type, ChessGameBaseState> _stateDictionary;
        private bool _didInit;
        private bool _isWhiteTurn;

        public Stack<Move> moveHistory { get; private set; }

        public ChessGame() {
            _board = new Board(8);
            _stateDictionary = new Dictionary<Type, ChessGameBaseState>() {
            };
        }

        public void Init() {
            _board.Init();
            moveHistory = new Stack<Move>();
            // currentState = new ChessGamePlayingState();
            // currentState.Enter(this);
            _didInit = true;
        }

        // todo: make a faster version with piece or pieceName as parameter
        public bool TryMakeMove(bool isFromWhite, Move move) {
            if (!_didInit) {
                Debug.LogError("ChessGame.TryMakeMove: Game not initialized!");
                return false;
            }
            var isSuccessful = _board.TryMakeMove(isFromWhite, move, _isWhiteTurn);
            if (isSuccessful) {
                _isWhiteTurn = !_isWhiteTurn;
                moveHistory.Push(move);
                OnBoardUpdate?.Invoke(this, _board.PieceList);
            }

            return isSuccessful;
        }

        // todo: make a faster version with piece or pieceName as parameter
        public List<Move> GetLegalMoves(bool isFromWhite) {
            if (!_didInit) {
                Debug.LogError("ChessGame.GetLegalMoves: Game not initialized!");
                return null;
            }
            
            return MoveGenerator.GetLegalMoves(_board, isFromWhite, _isWhiteTurn);
        }
    }


    public abstract class ChessGameBaseState {
        public abstract void Enter(ChessGame game);
        public abstract void Exit(ChessGame game);
    }
}