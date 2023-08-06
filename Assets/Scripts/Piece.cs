using System;
using Riptide;

public struct Piece : IMessageSerializable, IEquatable<Piece> {
    public static readonly Piece None = new("None", false);
    public string Name;
    public bool IsWhite;
        
    public Piece(string name, bool isWhite) {
        Name = name;
        IsWhite = isWhite;
    }

    public void Serialize(Message message) {
        message.Add(Name);
        message.Add(IsWhite);
    }

    public void Deserialize(Message message) {
        Name = message.GetString();
        IsWhite = message.GetBool();
    }

    public bool Equals(Piece other) {
        if (other.Name == None.Name) {
            return Name == None.Name;
        }
        return Name == other.Name && IsWhite == other.IsWhite;
    }

    public override bool Equals(object obj) {
        return obj is Piece other && Equals(other);
    }

    public override int GetHashCode() {
        return HashCode.Combine(Name, IsWhite);
    }
    
    public static bool operator ==(Piece left, Piece right) {
        return left.Equals(right);
    }
    
    public static bool operator !=(Piece left, Piece right) {
        return !left.Equals(right);
    }
}