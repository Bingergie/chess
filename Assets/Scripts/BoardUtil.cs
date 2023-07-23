using System.Collections.Generic;
using Core;
using UI;
using UnityEngine;

public static class BoardUtil {
    public const string fileNames = "abcdefgh";
    public const string rankNames = "12345678";
    
    public const string startFen = "rnbqkbnr/pppppppp/8/8/8/8/PPPPPPPP/RNBQKBNR";
    public const string emptyFen = "8/8/8/8/8/8/8/8";

    public static readonly Dictionary<char, string> pieceNames = new Dictionary<char, string> {
        {'K', "King"},
        {'Q', "Queen"},
        {'R', "Rook"},
        {'B', "Bishop"},
        {'N', "Knight"},
        {'P', "Pawn"}
    };

    public static string NameFromCoord(int fileIndex, int rankIndex) {
        return fileNames[fileIndex] + "" + rankNames[rankIndex];
    }
    
    public static string NameFromCoord(Coord coord) {
        return NameFromCoord(coord.fileIndex, coord.rankIndex);
    }
    
    public static Coord CoordFromName(string name) {
        return new Coord(fileNames.IndexOf(name[0]), rankNames.IndexOf(name[1]));
    }
    
    public static int IndexFromCoord(Coord coord) {
        return coord.rankIndex * GameManager.Width + coord.fileIndex;
    }
    
    public static Piece[] PiecesFromFen(string fen = startFen) {
        var pieceList = new List<Piece>();
        var fenParts = fen.Split(' ');
        var fenBoard = fenParts[0];
        foreach (var character in fenBoard) {
            if (character == '/') {
            } else if (char.IsDigit(character)) {
                for (int i = 0; i < int.Parse(character.ToString()); i++) 
                    pieceList.Add(default);
            } else {
                var pieceName = pieceNames[char.ToUpper(character)];
                var isWhite = char.IsUpper(character);
                pieceList.Add(new Piece(pieceName, isWhite));
            }
        }
        // reverse the list so that the pieces are in the correct order
        pieceList.Reverse();
        return pieceList.ToArray();
    }

    public static int CoordFromFileRank(int file, int rank) {
        return rank * GameManager.Width + file;
    }
}