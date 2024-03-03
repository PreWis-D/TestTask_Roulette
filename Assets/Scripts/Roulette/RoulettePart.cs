using TMPro;
using UnityEngine;

public class RoulettePart : MonoBehaviour
{
    [SerializeField] private RectTransform _pieceHolder;
    [SerializeField] private TMP_Text _rewardText;

    [HideInInspector] public int Index;
    [HideInInspector] public double Weight = 0f;

    public RectTransform PieceHolder => _pieceHolder;

    public int Reward { get; private set; }

    public void Init(int reward)
    {
        Reward = reward;
        UpdateText();
    }

    public void SetupRotate(Vector3 targetPositionLook, float angle)
    {
        _pieceHolder.transform.RotateAround(targetPositionLook, Vector3.back, angle);
    }

    private void UpdateText()
    {
        _rewardText.text = Reward.ToString();
    }
}