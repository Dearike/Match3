public interface IBoardModelProvider
{
    IBoardModel Model { get; }
    IPieceModel GetNewPieceModel();
}
