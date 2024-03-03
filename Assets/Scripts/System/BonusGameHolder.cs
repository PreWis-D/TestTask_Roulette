using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BonusGameHolder : MonoBehaviour
{
    [SerializeField] private RouletteConfig _rouletteConfig;

    [Space(20)]
    [SerializeField] private Roulette _roulette;
    [SerializeField] private RewardAnimator _rewardAnimator;

    [Space(20)]
    [SerializeField] private Canvas _staticCanvas;
    [SerializeField] private Canvas _dynamicCanvas;
    [SerializeField] private Button _spinButton;
    [SerializeField] private TMP_Text _textButton;
    [SerializeField] private Color _cooldownColor;

    private Color _defaultTextColor;
    private const string _spinText = "Испытать удачу";

    public void Init(Camera camera)
    {
        _staticCanvas.renderMode = RenderMode.ScreenSpaceCamera;
        _staticCanvas.worldCamera = camera;
        _dynamicCanvas.renderMode = RenderMode.ScreenSpaceCamera;
        _dynamicCanvas.worldCamera = camera;

        _defaultTextColor = _textButton.color;

        Subscrube();

        _rewardAnimator.Init(_rouletteConfig);
        _roulette.Init(_rewardAnimator, _rouletteConfig);
    }

    private void Subscrube()
    {
        _spinButton.onClick.AddListener(() => { _roulette.Spin(); });
        _roulette.RouletteActivated += OnRouletteActivated;
        _roulette.SpinStarted += OnSpinStarted;
        _roulette.PieceFound += OnPieceFound;
        _roulette.SpinEnded += OnSpinEnded;
        _roulette.CooldownUpdated += OnCooldownUpdated;
    }

    private void OnCooldownUpdated(int value)
    {
        _spinButton.interactable = false;
        _textButton.color = _cooldownColor;
        _textButton.text = value.ToString();
    }

    private void OnRouletteActivated()
    {
        _textButton.color = _defaultTextColor;
        _spinButton.interactable = true;
        _textButton.text = _spinText;
    }

    private void OnPieceFound(RoulettePart part)
    {
        Debug.Log(part.Reward);
    }

    private void OnSpinEnded(RoulettePart part)
    {
        _spinButton.interactable = false;
    }

    private void OnSpinStarted()
    {
        _spinButton.interactable = false;
    }

    private void Unsubscrube()
    {
        _spinButton.onClick.RemoveListener(() => { _roulette.Spin(); });
        _roulette.RouletteActivated -= OnRouletteActivated;
        _roulette.SpinStarted -= OnSpinStarted;
        _roulette.PieceFound -= OnPieceFound;
        _roulette.SpinEnded -= OnSpinEnded;
        _roulette.CooldownUpdated -= OnCooldownUpdated;
    }

    private void OnDestroy()
    {
        Unsubscrube();
    }
}
