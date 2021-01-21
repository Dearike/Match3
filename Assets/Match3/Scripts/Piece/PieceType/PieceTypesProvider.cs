using UnityEngine;

public class PieceTypesProvider : MonoBehaviour, IPieceTypesProvider
{
    [SerializeField]
    private int _countOfTypes = 5;

    private PieceType[] _types;

    public PieceType[] Types => _types;

    public void Initialize()
    {
        _types = new PieceType[_countOfTypes];

        for (int i = 0; i < _countOfTypes; i++)
        {
            var j = i;
            Types[i] = new PieceType(j);
        }
    }
}
