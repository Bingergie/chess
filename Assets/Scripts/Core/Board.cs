using System.Collections.Generic;
using UnityEngine;

namespace Core {
    public struct Board {
        public readonly int Width;
        public readonly int Height;
        public Piece[] PieceList;
        private bool _didInit;

        public Board(int width = 8, int height = 8, string fen = BoardUtil.StartFen) {
            this.Width = width;
            this.Height = height;
            PieceList = new Piece[width * height];
            _didInit = false;
        }

        public bool TryMakeMove(bool isFromWhite, Move move, bool isWhiteTurn) {
            if (!_didInit) {
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
            PieceList[move.ToIndex] = PieceList[move.FromIndex];
            PieceList[move.FromIndex] = default;
            return true;
        }
        
        public void Init() {
            SetupBoard();
        }

        private void SetupBoard(string fen = BoardUtil.StartFen) {
            PieceList = BoardUtil.PiecesFromFen(fen);
            if (PieceList.Length != Width * Height) {
                Debug.LogError("FEN does not match board dimensions!");
                Debug.Log(PieceList.Length + " != " + Width * Height);
            }
            _didInit = true;
        }
    }
}