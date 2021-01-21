using System;
using UnityEngine;
using DG.Tweening;

[RequireComponent(typeof(BoxCollider))]
public class Piece : AbstractPiece
{
    [SerializeField]
    private GameObject _bonusSprite;

    private bool _IsDragging;

    private Camera _camera;

    public Bonus Bonus { get; set; }

    public Action<Piece, SwapDirection> OnSwaped = (piece, direction) => { };
    public Action OnDragModeSwaped = () => { };

    private void Awake()
    {
        _camera = Camera.main;
    }

    protected override void SetSprite()
    {
        _spriteRenderer.sprite = Resources.Load<Sprite>($"Graphics/Shapes/{_model.Type.ID}");
        _bonusSprite.SetActive(false);
    }

    protected override void Appear()
    {
        DOTween.Sequence()
            .Append(transform.DOScale(1f, Settings.Instance.TimeToAppear))
            .Join(_spriteRenderer.DOFade(1f, Settings.Instance.TimeToAppear));
    }

    public override void Fade(float time)
    {
        _inUse = false;
        DOTween.Sequence()
                .Append(transform.DOScale(0.2f, time))
                .Join(_spriteRenderer.DOFade(0f, time));
    }

    public void ActivateBonus(Bonus bonus)
    {
        Bonus = bonus;
        _bonusSprite.SetActive(true);
    }

	private void Update()
	{
		if (Settings.Instance.DragMode && _IsDragging)
		{
            CheckDrag(Input.mousePosition);
		}
	}

	private void OnMouseDown()
	{
        _IsDragging = true;
	}

	private void OnMouseUp()
	{
		if (!Settings.Instance.DragMode)
		{
			CheckDrag(Input.mousePosition);
		}
		else
		{
            OnDragModeSwaped?.Invoke();
        }

        _IsDragging = false;
	}
    
	private void CheckDrag(Vector2 position)
	{
		if (!_IsDragging) return;

		position = _camera.ScreenToWorldPoint(position);

		var x = transform.position.x;
		var y = transform.position.y;

		var diffX = Mathf.Abs(x - position.x);
		var diffY = Mathf.Abs(y - position.y);

		var right = position.x > x + Settings.Instance.PixelDragThreshold;
		var left = position.x < x - Settings.Instance.PixelDragThreshold;
		var up = position.y > y + Settings.Instance.PixelDragThreshold;
		var down = position.y < y - Settings.Instance.PixelDragThreshold;

		if (up && (right || left))
		{
			up = diffY >= diffX;
			if (up) right = left = false;
		}
		if (down && (right || left))
		{
			down = diffY >= diffX;
			if (down) right = left = false;
		}

		SwapDirection direction = SwapDirection.NONE;

		if (up) direction = SwapDirection.UP;
		if (down) direction = SwapDirection.DOWN;
		if (left) direction = SwapDirection.LEFT;
		if (right) direction = SwapDirection.RIGHT;

        if (direction != SwapDirection.NONE)
            OnSwaped?.Invoke(this, direction);
	}
}
