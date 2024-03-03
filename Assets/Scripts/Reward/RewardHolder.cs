using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

[Serializable]
public class RewardHolder
{
    [Header("Reward settings")]
    [SerializeField] private Image _rewardIcon;
    [SerializeField] private Sprite[] _rewardSprites;
    [SerializeField] private TMP_Text _rewardText;
    [SerializeField] private SmoothedTextValue _smoothedTextValue;

    private Roulette _roulette;
    private Randomizer _randomizer;
    private int _currentReward = 0;
    private Action _callback;

    public Image RewardIcon => _rewardIcon;

    public RewardType CurrentRewardType { get; private set; }

    public void Init(Roulette roulette)
    {
        _roulette = roulette;

        _randomizer = new Randomizer();
    }

    public void AddReward(int reward, Action callback)
    {
        _callback = callback;

        _smoothedTextValue.StartSmooth(_rewardText, _currentReward, _currentReward + reward);
        _currentReward += reward;

        if (_currentReward >= _roulette.CurrentRoulettePart.Reward)
        {
            _callback?.Invoke();
            _callback = null;
        }
    }

    public void GenerateNewReward()
    {
        int[] randomNumbers = _randomizer.GenerateRandomNumbers(
            _roulette.RouletteParts.Length
            , _roulette.RouletteConfig.MinRandomValue
            , _roulette.RouletteConfig.MaxRandomValue);
        for (int i = 0; i < _roulette.RouletteParts.Length; i++)
            _roulette.RouletteParts[i].Init(randomNumbers[i]);

        CurrentRewardType = _randomizer.GenerateRandomRewardType();
        _rewardIcon.sprite = _rewardSprites[CurrentRewardType.GetHashCode() - 1];
    }

    public void TryDeactivateText()
    {
        if (_rewardText.gameObject.activeSelf)
        {
            _currentReward = 0;
            _rewardText.text = _currentReward.ToString();
            ChangeView(false);
        }
    }

    public void ChangeView(bool isTextView)
    {
        _rewardIcon.gameObject.SetActive(!isTextView);
        _rewardText.gameObject.SetActive(isTextView);
    }
}