using System.Collections.Generic;
using UnityEngine;

public class RandomBoardModelProvider : MonoBehaviour, IBoardModelProvider
{
    [SerializeField]
    private int _rows, _columns;

    [SerializeField]
    private PieceTypesProvider _pieceTypesProvider;

    private IBoardModel _model;

    public IBoardModel Model => _model;

    public void Awake()
	{
        _pieceTypesProvider.Initialize();

        _model = new BoardModel(_rows, _columns);

        PieceType[] previousLeft = new PieceType[_columns];
        PieceType previousBelow = null;

        Model.PieceModels = new PieceModel[_rows][];
		for (int x = 0; x < _rows; x++)
		{
            Model.PieceModels[x] = new PieceModel[_columns];
			for (int y = 0; y < _columns; y++)
			{
				List<PieceType> possibleTypes = new List<PieceType>();
				possibleTypes.AddRange(_pieceTypesProvider.Types);

				possibleTypes.Remove(previousLeft[y]);
				possibleTypes.Remove(previousBelow);

                PieceType type = possibleTypes[Random.Range(0, possibleTypes.Count)];
                PieceModel pieceModel = new PieceModel(type);

				previousLeft[y] = type;
				previousBelow = type;

                Model.PieceModels[x][y] = pieceModel;
			}
		}
	}

    public IPieceModel GetNewPieceModel()
    {
        return new PieceModel(
            _pieceTypesProvider.Types[Random.Range(0, _pieceTypesProvider.Types.Length)]);
    }
}
