public class BoardModel : IBoardModel
{
    public PieceModel[][] PieceModels { get; set; }
    public int Rows { get; }
    public int Columns { get; }

    public BoardModel(int rows, int columns)
    {
        Rows = rows;
        Columns = columns;
    }
}
