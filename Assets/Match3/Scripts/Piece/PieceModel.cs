using System;

public class PieceModel : IPieceModel
{
    public PieceType Type { get; }

    public PieceModel(PieceType type)
    {
        Type = type;
    }

    public int CompareTo(object other)
    {
        if (other == null) return 1;

        PieceModel otherPiece = other as PieceModel;
        if (otherPiece != null)
            return this.Type.ID.CompareTo(otherPiece.Type.ID);
        else
            throw new ArgumentException("Object is not a PieceType");
    }
}
