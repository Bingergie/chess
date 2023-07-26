public readonly struct Move {
    public readonly int FromIndex;
    public readonly int ToIndex;

    public Move(int fromIndex, int toIndex) {
        FromIndex = fromIndex;
        ToIndex = toIndex;
    }
        
    public Move(Coord fromCoord, Coord toCoord) {
        FromIndex = BoardUtil.IndexFromCoord(fromCoord);
        ToIndex = BoardUtil.IndexFromCoord(toCoord);
    }
}