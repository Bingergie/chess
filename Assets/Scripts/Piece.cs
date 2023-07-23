public readonly struct Piece {
    public readonly string name;
    public readonly bool isWhite;
        
    public Piece(string name, bool isWhite) {
        this.name = name;
        this.isWhite = isWhite;
    }
}