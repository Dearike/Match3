using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public abstract class AbstractPiece : MonoBehaviour
{
    [SerializeField]
    protected SpriteRenderer _spriteRenderer;

    protected IPieceModel _model;
    protected IMatch _match;

    protected bool _inUse;

    public IPieceModel Model => _model;
    public IMatch Match => _match;

    public int Row { get; set; }
    public int Column { get; set; }
    public bool InUse => _inUse;
    public Vector3 Size => _spriteRenderer.bounds.size;
    
    public void SetModel(IPieceModel model, int row, int column)
	{
		_model = model;
        _match = new Match();

        Row = row;
        Column = column;

        _inUse = true;

        SetSprite();
        Appear();
    }

    protected abstract void SetSprite();
    protected abstract void Appear();
    public abstract void Fade(float time);
}
