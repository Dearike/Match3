public interface IBoardModel
{
    PieceModel[][] PieceModels { get; set; }
    int Rows { get; }
    int Columns { get; }
}
