public readonly struct Move {
    public readonly int fromIndex;
    public readonly int toIndex;

    public Move(int fromIndex, int toIndex) {
        this.fromIndex = fromIndex;
        this.toIndex = toIndex;
    }
        
    public Move(Coord fromCoord, Coord toCoord) {
        fromIndex = BoardUtil.IndexFromCoord(fromCoord);
        toIndex = BoardUtil.IndexFromCoord(toCoord);
    }
}