using System;
using System.Collections.Generic;

public static class BoardUtil {
    public const string FileNames = "abcdefgh";
    public const string RankNames = "12345678";
    
    public const string StartFen = "rnbqkbnr/pppppppp/8/8/8/8/PPPPPPPP/RNBQKBNR";
    public const string EmptyFen = "8/8/8/8/8/8/8/8";

    public static readonly Dictionary<char, string> PieceNames = new Dictionary<char, string> {
        {'K', "King"},
        {'Q', "Queen"},
        {'R', "Rook"},
        {'B', "Bishop"},
        {'N', "Knight"},
        {'P', "Pawn"}
    };

    public static string NameFromCoord(int fileIndex, int rankIndex) {
        return FileNames[fileIndex] + "" + RankNames[rankIndex];
    }
    
    public static string NameFromCoord(Coord coord) {
        return NameFromCoord(coord.FileIndex, coord.RankIndex);
    }
    
    public static Coord CoordFromName(string name) {
        return new Coord(FileNames.IndexOf(name[0]), RankNames.IndexOf(name[1]));
    }
    
    public static Coord CoordFromIndex(int index) {
        return new Coord(index % GameManager.Width, index / GameManager.Width);
    }
    
    public static int IndexFromCoord(Coord coord) {
        return coord.RankIndex * GameManager.Width + coord.FileIndex;
    }
    
    public static int IndexFromCoord(int file, int rank) {
        return rank * GameManager.Width + file;
    }
    
    public static Piece[] PiecesFromFen(string fen = StartFen) {
        var pieceList = new Piece[64];
        var fenParts = fen.Split(' ');
        var fenBoard = fenParts[0].Split('/');
        var currentRank = 7;
        foreach (var rank in fenBoard) {
            var currentFile = 0;
            foreach (var character in rank) {
                if (char.IsDigit(character)) {
                    for (int i = 0; i < int.Parse(character.ToString()); i++) {
                        pieceList[IndexFromCoord(currentFile, currentRank)] = Piece.None;
                        currentFile++;
                    }
                } else {
                    var pieceName = PieceNames[char.ToUpper(character)];
                    var isWhite = char.IsUpper(character);
                    pieceList[IndexFromCoord(currentFile, currentRank)] = new Piece(pieceName, isWhite);
                    currentFile++;
                }
            }
            currentRank--;
        }
        return pieceList;
        
        /*var pieceList = new List<Piece>();
        var fenParts = fen.Split(' ');
        var fenBoard = fenParts[0];
        foreach (var character in fenBoard) {
            if (character == '/') {
            } else if (char.IsDigit(character)) {
                for (int i = 0; i < int.Parse(character.ToString()); i++) 
                    pieceList.Add(Piece.None);
            } else {
                var pieceName = PieceNames[char.ToUpper(character)];
                var isWhite = char.IsUpper(character);
                pieceList.Add(new Piece(pieceName, isWhite));
            }
        }
        // reverse the list so that the pieces are in the correct order
        //pieceList.Reverse();
        // flip the board so that white is on the bottom
        return pieceList.ToArray();*/
    }
}