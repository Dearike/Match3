using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public enum SwapDirection
{
    LEFT,
    RIGHT,
    UP,
    DOWN,
    NONE
}

public enum GravityDirection
{
    DOWN,
    UP
}

public class Board : MonoBehaviour
{
    [SerializeField]
    public bool _swapBack;

    [SerializeField]
    public bool _diagonalMatches;
    
    [SerializeField]
    public Piece _pieceViewPrefab;

    [SerializeField]
    private RandomBoardModelProvider _boardModelProvider;

	private IBoardModel _boardModel;
    private Piece[][] _board;

    private Piece _firstSwappedPiece;
    private Piece _secondSwappedPiece;

    private SwapDirection _currentDirection;
    private GravityDirection _currentGravityDirection;

    private int _rows;
    private int _columns;
    
    public bool CanMove { get; set; }
    public bool NeedCheckMatches { get; set; }
    public bool GameIsOver { get; set; }

    public void Start()
	{
        _boardModel = _boardModelProvider.Model;

        CreateBoardView(_pieceViewPrefab.Size.x, _pieceViewPrefab.Size.y);

		CanMove = true;
		GameIsOver = false;
	}
    
	private void CreateBoardView(float xOffset, float yOffset)
	{
		float startX = transform.position.x;
		float startY = transform.position.y;

        _rows = _boardModel.Rows;
        _columns = _boardModel.Columns;
        _board = new Piece[_rows][];

		for (int x = 0; x < _rows; x++)
		{
            _board[x] = new Piece[_columns];
			for (int y = 0; y < _columns; y++)
			{
				var tile = Instantiate(
                    _pieceViewPrefab,
					new Vector3(startX + (xOffset * x),
								startY + (yOffset * y),
								2),
                    _pieceViewPrefab.transform.rotation);

                tile.OnSwaped += TrySwapPieces;
                tile.OnDragModeSwaped += StartCheckForMatchesCoroutine;
				tile.SetModel(_boardModel.PieceModels[y][x], y, x);
                _board[x][y] = tile;
			}
		}
	}

    public void TrySwapPieces(Piece pieceView, SwapDirection swapDirection)
    {
        if(CanMove)
            SwapPieces(pieceView, swapDirection);
    }
    
	public void SwapPieces(Piece piece, SwapDirection direction, bool checkMatches = true)
	{
        _firstSwappedPiece = piece;
		_currentDirection = direction;

		bool validMove = CheckForValidMove(piece, direction);

		if (!validMove)
		{
			if (!Settings.Instance.DragMode)
				piece.gameObject.transform.DOShakePosition(0.5f, 0.3f);
			return;
		}

		CanMove = false;

        _secondSwappedPiece = GetPieceByDirection(piece, direction);

		var firstPiecePosition = _firstSwappedPiece.transform.position;
		var secondPiecePosition = _secondSwappedPiece.transform.position;

		DOTween.Sequence()
            .Append(piece.transform.DOMove(secondPiecePosition, Settings.Instance.TimeToSwap))
			.Join(_secondSwappedPiece.transform.DOMove(firstPiecePosition, Settings.Instance.TimeToSwap))
			.SetEase(Ease.OutCirc)
			.OnComplete(() =>
			{
                SwapPieceData(_firstSwappedPiece, _secondSwappedPiece, checkMatches);
			});
	}
    
	private void SwapPieceData(Piece piece, Piece otherPiece, bool checkMatches = true)
	{
		var pieceRow = piece.Row;
		var pieceColumn = piece.Column;

		piece.Row = otherPiece.Row;
		piece.Column = otherPiece.Column;

		otherPiece.Row = pieceRow;
		otherPiece.Column = pieceColumn;

		_board[piece.Column][piece.Row] = piece;
		_board[otherPiece.Column][otherPiece.Row] = otherPiece;

		CanMove = true;

		if (NeedCheckMatches || !Settings.Instance.DragMode && checkMatches)
		{
			NeedCheckMatches = false;
			StartCoroutine(CheckForMatches());
		}
	}
    
    public IEnumerator CheckForMatches(bool needSwapBack = true)
	{
		CanMove = false;

		bool hasMatches = false;

		if (GameIsOver) yield break;

		for (int x = 0; x < _rows; x++)
		{
			for (int y = 0; y < _columns; y++)
			{
				var horizontal = CheckMatches(x, y, MatchType.HORIZONTAL);
				var vertical = CheckMatches(x, y, MatchType.VERTICAL);
				var dright = _diagonalMatches && CheckMatches(x, y, MatchType.DIAGONAL_RIGHT);
				var dleft = _diagonalMatches && CheckMatches(x, y, MatchType.DIAGONAL_LEFT);

				if (!hasMatches)
					hasMatches = horizontal || vertical || dright || dleft;
			}
		}

		if (!hasMatches && _swapBack && !Settings.Instance.DragMode && needSwapBack)
		{
            SwapPieces(_firstSwappedPiece, GetOppositeDirection(_currentDirection), false);
			CanMove = true;
		}

		yield return new WaitForSeconds(Settings.Instance.TimeToDestroy);

		CanMove = true;

		if (hasMatches)
		{
			CanMove = false;
			StartCoroutine(ShiftPieces());
		}
	}
    
	private bool CheckMatches(int x, int y, MatchType matchType)
	{
		var addX = (matchType == MatchType.DIAGONAL_LEFT ? -1 : matchType == MatchType.HORIZONTAL ? 1 : 0);
		var addY = (matchType == MatchType.VERTICAL ? 1 : 0);
		var pX = x + addX;
		var pY = y + addY;
		var hasMatches = false;

		List<Piece> pieceList = new List<Piece>(){ _board[x][y] };

		var currentPiece = _board[x][y];

		while (InBounds(pX, pY)
            && currentPiece.Model.CompareTo(_board[pX][pY].Model) == 0
            && !_board[pX][pY].Match.HasMatch(matchType))
		{
			pieceList.Add(_board[pX][pY]);
			pX = pX + addX;
			pY = pY + addY;
		}

        if (pieceList.Count > 3)
        {
            Piece bonusablePiece = null;

            if(pieceList.Contains(_firstSwappedPiece))
            {
                bonusablePiece = _firstSwappedPiece;
                pieceList.Remove(_firstSwappedPiece);
            }
            else if (pieceList.Contains(_secondSwappedPiece))
            {
                bonusablePiece = _secondSwappedPiece;
                pieceList.Remove(_secondSwappedPiece);
            }
            else
            {
                bonusablePiece = currentPiece;
                pieceList.Remove(currentPiece);
            }

            bonusablePiece.ActivateBonus(Bonus.CHANGE_GRAVITY);
        }

        if (pieceList.Count > 2)
		{
			pieceList.ForEach(p => p.Match.SetMatch(matchType));
            ApplyBonuses(pieceList);
			FadePieces(pieceList);
			hasMatches = true;
		}

		return hasMatches;
	}
    
	private IEnumerator ShiftPieces()
	{
        if (_currentGravityDirection == GravityDirection.DOWN)
            ShiftDownPieces();
        else
            ShiftUpPieces();

        yield return new WaitForSeconds(Settings.Instance.TimeToDestroy);

		for (int x = 0; x < _rows; x++)
		{
			for (int y = 0; y < _columns; y++)
			{
				if (_board[x][y].InUse) continue;
				_board[x][y].SetModel(
                    _boardModelProvider.GetNewPieceModel(), y, x);
			}
		}

		yield return new WaitForSeconds(Settings.Instance.TimeToDestroy);

		StartCoroutine(CheckForMatches(false));
	}

    private void ShiftDownPieces()
    {
        float offset = _pieceViewPrefab.Size.y;

        for (int x = 0; x < _rows; x++)
        {
            int shifts = 0;
            for (int y = 0; y < _columns; y++)
            {
                if (!_board[x][y].InUse)
                {
                    shifts++;
                    continue;
                }

                if (shifts == 0) continue;

                _board[x][y].transform
                    .DOMoveY(_board[x][y].transform.position.y - (offset * shifts), Settings.Instance.TimeToDestroy)
                    .SetEase(Ease.InExpo);

                var holder = _board[x][y - shifts];

                _board[x][y - shifts] = _board[x][y];
                _board[x][y - shifts].Row = y - shifts;

                _board[x][y] = holder;
                _board[x][y].transform.position = _board[x][y - shifts].transform.position;
            }
        }
    }

    private void ShiftUpPieces()
    {
        float offset = _pieceViewPrefab.Size.y;

        for (int x = 0; x < _rows; x++)
        {
            int shifts = 0;
            for (int y = _columns - 1; y >= 0; y--)
            {
                if (!_board[x][y].InUse)
                {
                    shifts++;
                    continue;
                }

                if (shifts == 0) continue;

                _board[x][y].transform
                    .DOMoveY(_board[x][y].transform.position.y + (offset * shifts), Settings.Instance.TimeToDestroy)
                    .SetEase(Ease.InExpo);

                var holder = _board[x][y + shifts];

                _board[x][y + shifts] = _board[x][y];
                _board[x][y + shifts].Row = y + shifts;

                _board[x][y] = holder;
                _board[x][y].transform.position = _board[x][y + shifts].transform.position;
            }
        }
    }

    private void ApplyBonuses(List<Piece> pieceList)
    {
        pieceList.ForEach(x => {
            if(x.Bonus == Bonus.CHANGE_GRAVITY)
                _currentGravityDirection = _currentGravityDirection == GravityDirection.UP
                    ? GravityDirection.DOWN
                    : GravityDirection.UP;
        });
    }

    private void FadePieces(List<Piece> pieceList)
	{
		pieceList.ForEach(x => {
			x.Fade(Settings.Instance.TimeToDestroy);
		});
	}
    
	private Piece GetPieceByDirection(Piece piece, SwapDirection direction)
	{
		var column = piece.Column + (direction == SwapDirection.LEFT ? (-1) : direction == SwapDirection.RIGHT ? 1 : 0);
		var row = piece.Row + (direction == SwapDirection.DOWN ? (-1) : direction == SwapDirection.UP ? 1 : 0);
		return _board[column][row];
	}
    
	private bool CheckForValidMove(Piece piece, SwapDirection direction)
	{
		return !(
            direction == SwapDirection.LEFT  &&  piece.Column == 0 ||
			direction == SwapDirection.RIGHT &&  piece.Column == _columns - 1 ||
			direction == SwapDirection.UP    &&  piece.Row == _rows - 1 ||
			direction == SwapDirection.DOWN  &&  piece.Row == 0);
	}
    
	private SwapDirection GetOppositeDirection(SwapDirection direction)
	{
		switch (direction)
		{
			case SwapDirection.DOWN:
				return SwapDirection.UP;
			case SwapDirection.UP:
				return SwapDirection.DOWN;
			case SwapDirection.LEFT:
				return SwapDirection.RIGHT;
			case SwapDirection.RIGHT:
				return SwapDirection.LEFT;
		}

		return SwapDirection.NONE;
    }

    private bool InBounds(int x, int y)
    {
        return x >= 0 && x < _columns && y >= 0 && y < _rows;
    }

    private void StartCheckForMatchesCoroutine()
    {
        if (CanMove)
            StartCoroutine(CheckForMatches());
        else
            NeedCheckMatches = true;
    }

    protected virtual void OnDestroy()
    {
        for (int x = 0; x < _rows; x++)
            for (int y = 0; y < _columns; y++)
            {
                _board[x][y].OnSwaped -= TrySwapPieces;
                _board[x][y].OnDragModeSwaped -= StartCheckForMatchesCoroutine;
            }
    }
}
