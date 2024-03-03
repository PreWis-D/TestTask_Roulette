using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class RewardIcon : MonoBehaviour
{
    [SerializeField] private Image _icon;

    [Space(20)]
    [Header("AnimationSetting")]
    [SerializeField] private float _durationMove = 0.5f;
    [SerializeField] private float _durationStartScale = 0.5f;
    [SerializeField] private float _minDelay = 1f;
    [SerializeField] private float _maxDelay = 2.5f;

    private Sequence _sequence;
    private Camera _camera;
    private int _reward;
    private Roulette _roulette;
    private RectTransform _rectTransform;
    private RectTransform _targetTransform;
    private Randomizer _randomizer;
    private Vector3 _randomRadiusPosition;

    private void Awake()
    {
        _rectTransform = GetComponent<RectTransform>();
        _randomizer = new Randomizer();
    }

    public void Init(int reward, RectTransform moneyPosition, Roulette roulette, RectTransform rectCanvas,
        float minRadius, float maxRadius, Sprite sprite)
    {
        if (_sequence != null && _sequence.active)
            _sequence.Kill();

        _icon.sprite = sprite;
        _roulette = roulette;
        _reward = reward;
        _targetTransform = moneyPosition;
        _camera = Camera.main;

        Vector2 canvasSize = rectCanvas.sizeDelta;
        Vector2 viewPortPos3d = _camera.WorldToViewportPoint(moneyPosition.position);
        Vector2 viewPortRelative = new Vector2(viewPortPos3d.x - 0.5f, viewPortPos3d.y - 0.5f);
        Vector2 moneyScreenPosition = new Vector2(viewPortRelative.x * canvasSize.x, viewPortRelative.y * canvasSize.y);

        _rectTransform.localPosition = moneyScreenPosition + (canvasSize / 2);
        _rectTransform.localScale = Vector3.zero;
        _randomRadiusPosition = (Vector3)_randomizer.GenerateRandomRadiusPosition(minRadius, maxRadius);
    }

    public void ShowCoin()
    {
        AnimateMoving();
    }

    private void AnimateMoving()
    {
        transform.SetParent(_targetTransform);
        _sequence = DOTween.Sequence();
        _sequence.Append(_rectTransform.DOScale(Vector3.one, _durationStartScale).SetEase(Ease.Linear));
        _sequence.Join(_rectTransform.DOLocalMove(Vector3.zero + _randomRadiusPosition, _durationMove).SetEase(Ease.Linear));
        _sequence.AppendInterval(Random.Range(_minDelay, _maxDelay));
        _sequence.Append(_rectTransform.DOMove(_roulette.transform.position, _durationMove).SetEase(Ease.Linear));
        _sequence.Join(_rectTransform.DOScale(Vector3.zero, _durationMove).SetEase(Ease.Linear));
        _sequence.OnComplete(() =>
        {
            _roulette.AddReward(_reward);
            gameObject.SetActive(false);
        });
    }
}
