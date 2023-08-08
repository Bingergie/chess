using System;
using Riptide;

public struct Coord : IEquatable<Coord>, IMessageSerializable {
    public static readonly Coord None = default;

    public int FileIndex;
    public int RankIndex;

    public Coord (int fileIndex, int rankIndex) {
        FileIndex = fileIndex;
        RankIndex = rankIndex;
    }

    public bool IsLightSquare () {
        return (FileIndex + RankIndex) % 2 != 0;
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
    
    public static bool operator ==(Coord left, Coord right) {
        return left.Equals(right);
    }
    
    public static bool operator !=(Coord left, Coord right) {
        return !left.Equals(right);
    }

    public void Serialize(Message message) {
        message.Add(FileIndex);
        message.Add(RankIndex);
    }

    public void Deserialize(Message message) {
        FileIndex = message.GetInt();
        RankIndex = message.GetInt();
    }
}