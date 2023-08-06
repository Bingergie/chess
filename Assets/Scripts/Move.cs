using Riptide;

public struct Move : IMessageSerializable {
    public int FromIndex;
    public int ToIndex;

    public Move(int fromIndex, int toIndex) {
        FromIndex = fromIndex;
        ToIndex = toIndex;
    }

    public Move(Coord fromCoord, Coord toCoord) {
        FromIndex = BoardUtil.IndexFromCoord(fromCoord);
        ToIndex = BoardUtil.IndexFromCoord(toCoord);
    }

    public void Serialize(Message message) {
        message.Add(FromIndex);
        message.Add(ToIndex);
    }

    public void Deserialize(Message message) {
        FromIndex = message.GetInt();
        ToIndex = message.GetInt();
    }
}