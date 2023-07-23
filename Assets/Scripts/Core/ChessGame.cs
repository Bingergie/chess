using System;
using System.Collections.Generic;
using UnityEngine;

namespace Core {
    public class ChessGame {
        public event EventHandler<Piece[]> OnBoardUpdate;

        private Board board;
        private ChessGameBaseState currentState;
        private Dictionary<Type, ChessGameBaseState> stateDictionary;
        private bool didInit;
        private bool isWhiteTurn;

        public Stack<Move> moveHistory { get; private set; }

        public ChessGame() {
            board = new Board(8);
            stateDictionary = new Dictionary<Type, ChessGameBaseState>() {
            };
        }

        public void Init() {
            board.Init();
            moveHistory = new Stack<Move>();
            // currentState = new ChessGamePlayingState();
            // currentState.Enter(this);
            didInit = true;
        }

        // todo: make a faster version with piece or pieceName as parameter
        public bool TryMakeMove(bool isFromWhite, Move move) {
            if (!didInit) {
                Debug.LogError("ChessGame.TryMakeMove: Game not initialized!");
                return false;
            }
            var isSuccessful = board.TryMakeMove(isFromWhite, move);
            if (isSuccessful) {
                moveHistory.Push(move);
                OnBoardUpdate?.Invoke(this, board.pieceList);
            }

            return isSuccessful;
        }

        // todo: make a faster version with piece or pieceName as parameter
        public List<Move> GetLegalMoves(bool isFromWhite) {
            if (!didInit) {
                Debug.LogError("ChessGame.GetLegalMoves: Game not initialized!");
                return null;
            }
            
            return MoveGenerator.GetLegalMoves(board, isFromWhite);
        }
    }


    public abstract class ChessGameBaseState {
        public abstract void Enter(ChessGame game);
        public abstract void Exit(ChessGame game);
    }
}