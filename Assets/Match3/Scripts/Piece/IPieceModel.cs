using System;

public interface IPieceModel : IComparable
{
    PieceType Type { get; }
}
