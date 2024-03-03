using UnityEngine;

public class RouletteViewer
{
    private Roulette _roulette;

    private Vector2 _pieceMinSize = new Vector2(81f, 146f);
    private Vector2 _pieceMaxSize = new Vector2(144f, 213f);
    private int _piecesMin = 2;
    private int _piecesMax = 12;

    public float PieceAngle {  get; private set; }
    public float HalfPieceAngle { get; private set; }

    private double _accumulatedWeight;
    private System.Random rand = new System.Random();

    public RouletteViewer(Roulette roulette, int piecesAmount)
    {
        _roulette = roulette;

        PieceAngle = 360 / piecesAmount;
        HalfPieceAngle = PieceAngle / 2f;
    }

    public int GetRandomPieceIndex()
    {
        double r = rand.NextDouble() * _accumulatedWeight;

        for (int i = 0; i < _roulette.RouletteParts.Length; i++)
            if (_roulette.RouletteParts[i].Weight >= r)
                return i;

        return 0;
    }

    public void CreateWheel(RectTransform rectTransform)
    {
        RectTransform rt = rectTransform;
        float pieceWidth = Mathf.Lerp(_pieceMinSize.x, _pieceMaxSize.x, 1f - Mathf.InverseLerp(_piecesMin, _piecesMax, _roulette.RouletteParts.Length));
        float pieceHeight = Mathf.Lerp(_pieceMinSize.y, _pieceMaxSize.y, 1f - Mathf.InverseLerp(_piecesMin, _piecesMax, _roulette.RouletteParts.Length));
        rt.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, pieceWidth);
        rt.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, pieceHeight);

        CalculateWeightsAndIndices();
    }

    private void CalculateWeightsAndIndices()
    {
        for (int i = 0; i < _roulette.RouletteParts.Length; i++)
        {
            RoulettePart piece = _roulette.RouletteParts[i];

            _accumulatedWeight += i;
            piece.Weight = _accumulatedWeight;

            piece.Index = i;
        }
    }
}