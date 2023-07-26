using System;

public readonly struct Coord : IComparable<Coord>, IEquatable<Coord> {

    public readonly int FileIndex;
    public readonly int RankIndex;

    public Coord (int fileIndex, int rankIndex) {
        FileIndex = fileIndex;
        RankIndex = rankIndex;
    }

    public bool IsLightSquare () {
        return (FileIndex + RankIndex) % 2 != 0;
    }

    public int CompareTo (Coord other) {
        return (FileIndex == other.FileIndex && RankIndex == other.RankIndex) ? 0 : 1;
    }
    
    public static bool operator == (Coord a, Coord b) {
        return a.CompareTo(b) == 0;
    }

    public static bool operator !=(Coord a, Coord b) {
        return !(a == b);
    }
    
    public bool Equals(Coord other) {
        return FileIndex == other.FileIndex && RankIndex == other.RankIndex;
    }

    public override bool Equals(object obj) {
        return obj is Coord other && Equals(other);
    }

    public override int GetHashCode() {
        return HashCode.Combine(FileIndex, RankIndex);
    }
}