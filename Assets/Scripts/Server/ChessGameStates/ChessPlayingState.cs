using Riptide;
using Shared;
using UnityEngine;

namespace Server.ChessGameStates {
    public class ChessPlayingState : ChessBaseState {
        public ChessPlayingState(ChessGame game) : base(game) {
        }

        public override void Enter() {
        }

        public override void MakeMove(Move move, bool isFromWhite) {
            if (isFromWhite != Game.IsWhiteTurn) {
                Debug.Log("not your turn");
            }
            // todo: check if move is legal
            Game.PushMove(move);
            var board = Game.Board;
            board[move.ToIndex] = board[move.FromIndex];
            board[move.FromIndex] = Piece.None;
            Game.SetBoard(board);
            Game.SetIsWhiteTurn(!Game.IsWhiteTurn);
        }
    }
}