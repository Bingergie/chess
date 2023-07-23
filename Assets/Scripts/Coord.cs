using System;

public readonly struct Coord : IComparable<Coord>, IEquatable<Coord> {

    public readonly int fileIndex;
    public readonly int rankIndex;

    public Coord (int fileIndex, int rankIndex) {
        this.fileIndex = fileIndex;
        this.rankIndex = rankIndex;
    }

    public bool IsLightSquare () {
        return (fileIndex + rankIndex) % 2 != 0;
    }

    public int CompareTo (Coord other) {
        return (fileIndex == other.fileIndex && rankIndex == other.rankIndex) ? 0 : 1;
    }
    
    public static bool operator == (Coord a, Coord b) {
        return a.CompareTo(b) == 0;
    }

    public static bool operator !=(Coord a, Coord b) {
        return !(a == b);
    }
    
    public bool Equals(Coord other) {
        return fileIndex == other.fileIndex && rankIndex == other.rankIndex;
    }

    public override bool Equals(object obj) {
        return obj is Coord other && Equals(other);
    }

    public override int GetHashCode() {
        return HashCode.Combine(fileIndex, rankIndex);
    }
}