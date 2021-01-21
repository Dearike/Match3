using UnityEngine;

public class Settings : MonoBehaviour
{
    [SerializeField]
    public bool _dragMode;

    [SerializeField]
    public float _timeToSwap = 0.3f;

    [SerializeField]
    public float _timeToFade = 0.3f;

    [SerializeField]
    public float _timeToAppear = 0.3f;

    [SerializeField]
    public float _pixelDragThreshold = 1f;

    public bool DragMode => _dragMode;
    public float TimeToSwap => _timeToSwap;
    public float TimeToDestroy => _timeToFade;
    public float TimeToAppear => _timeToAppear;
    public float PixelDragThreshold => _pixelDragThreshold;

    public static Settings Instance { get; private set; }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }
}
