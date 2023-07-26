using System.Collections.Generic;
using UnityEngine;

namespace Core {
    public struct Board {
        public readonly int width;
        public readonly int height;
        public Piece[] pieceList;
        private bool didInit;

        public Board(int width = 8, int height = 8, string fen = BoardUtil.startFen) {
            this.width = width;
            this.height = height;
            pieceList = new Piece[width * height];
            didInit = false;
        }

        public bool TryMakeMove(bool isFromWhite, Move move, bool isWhiteTurn) {
            if (!didInit) {
                Debug.LogError("Board.TryMakeMove: Board not initialized!");
                return false;
            }
            
            List<Move> moveList = MoveGenerator.GetLegalMoves(this, isFromWhite, isWhiteTurn);
            if (moveList.Contains(move)) {
                // todo: make move
                Debug.Log("Move made!: " + move.ToString());
                return true;
            }
            // todo: delete this
            if (isFromWhite != isWhiteTurn) {
                Debug.Log("Not your turn!");
                return false;
            }
            // make the move
            pieceList[move.toIndex] = pieceList[move.fromIndex];
            pieceList[move.fromIndex] = default;
            return true;
        }
        
        public void Init() {
            SetupBoard();
        }

        private void SetupBoard(string fen = BoardUtil.startFen) {
            pieceList = BoardUtil.PiecesFromFen(fen);
            if (pieceList.Length != width * height) {
                Debug.LogError("FEN does not match board dimensions!");
                Debug.Log(pieceList.Length + " != " + width * height);
            }
            didInit = true;
        }
    }
}