using System.Collections;
using System.Collections.Generic;

namespace Core {
    public static class MoveGenerator {

        /*public static List<Move> GetLegalMoves(Piece[] pieceList, bool isFromWhite) {
            List<Move> moveList = new List<Move>();
            for (int i = 0; i < pieceList.Length; i++) {
                if (pieceList[i].isWhite == isFromWhite) {
                    switch (pieceList[i].name) {
                        case "Pawn":
                            moveList.AddRange(GetPawnMoves(pieceList, pieceList[i], i));
                            break;
                        case "Knight":
                            moveList.AddRange(GetKnightMoves(pieceList, pieceList[i], i));
                            break;
                        case "Bishop":
                            moveList.AddRange(GetBishopMoves(pieceList, pieceList[i], i));
                            break;
                        case "Rook":
                            moveList.AddRange(GetRookMoves(pieceList, pieceList[i], i));
                            break;
                        case "Queen":
                            moveList.AddRange(GetQueenMoves(pieceList, pieceList[i], i));
                            break;
                        case "King":
                            moveList.AddRange(GetKingMoves(pieceList, pieceList[i], i));
                            break;
                    }
                }
            }

            return moveList;
        }*/

        public static List<Move> GetLegalMoves(Board board, bool isFromWhite, bool isWhiteTurn) {
            return new List<Move>() { new Move() };
        }
    }
}