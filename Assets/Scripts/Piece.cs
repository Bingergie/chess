public readonly struct Piece {
    public readonly string Name;
    public readonly bool IsWhite;
        
    public Piece(string name, bool isWhite) {
        Name = name;
        IsWhite = isWhite;
    }
}