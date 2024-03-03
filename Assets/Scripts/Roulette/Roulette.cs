using UnityEngine;
using System;
using System.Collections;

public class Roulette : MonoBehaviour
{
    [Space]
    [SerializeField] private RoulettePart _roulettePartPrefab;
    [SerializeField] private Transform _wheelCircle;
    [SerializeField] private Transform _wheelPiecesParent;
    [SerializeField] private RewardHolder _rewardHolder;

    private RouletteViewer _rouletteViewer;
    private RewardAnimator _rewardAnimator;
    private Randomizer _randomizer;
    private RouletteAnimator _rouletteAnimator;

    private bool _isSpinning = false;

    private int _currentCooldown;
    private int _delayBeforeCooldown = 2;

    public RouletteConfig RouletteConfig { get; private set; }
    public RoulettePart CurrentRoulettePart { get; private set; }
    public RoulettePart[] RouletteParts { get; private set; }
    public RouletteState State { get; private set; } = RouletteState.Cooldown;

    public Action RouletteActivated;
    public Action SpinStarted;
    public Action<RoulettePart> PieceFound;
    public Action<RoulettePart> SpinEnded;
    public Action<int> CooldownUpdated;

    public void Init(RewardAnimator rewardAnimator, RouletteConfig rouletteConfig)
    {
        _rewardAnimator = rewardAnimator;
        RouletteConfig = rouletteConfig;

        _rouletteViewer = new RouletteViewer(this, RouletteConfig.PartsAmount);
        _rouletteAnimator = new RouletteAnimator(
            _wheelCircle
            , RouletteConfig.SpinDuration
            , _rouletteViewer.PieceAngle
            , _rouletteViewer.HalfPieceAngle);
        _randomizer = new Randomizer();
        RouletteParts = new RoulettePart[RouletteConfig.PartsAmount];
        _rewardHolder.Init(this);

        int[] randomNumbers = _randomizer.GenerateRandomNumbers(
            RouletteParts.Length
            , RouletteConfig.MinRandomValue
            , RouletteConfig.MaxRandomValue);
        for (int i = 0; i < RouletteParts.Length; i++)
        {
            RouletteParts[i] = Instantiate(_roulettePartPrefab, _wheelPiecesParent.position, Quaternion.identity, _wheelPiecesParent);
            RouletteParts[i].Init(randomNumbers[i]);
            RouletteParts[i].SetupRotate(_wheelPiecesParent.position, _rouletteViewer.PieceAngle * i);
        }

        _rouletteViewer.CreateWheel(_roulettePartPrefab.PieceHolder);

        switch (State)
        {
            case RouletteState.ChoiceReward:
                Spin();
                break;
            case RouletteState.Cooldown:
                StartCoroutine(Cooldown());
                break;
        }

        _rewardHolder.ChangeView(false);
    }

    public void Spin()
    {
        if (_isSpinning) return;

        State = RouletteState.ChoiceReward;

        _isSpinning = true;
        SpinStarted?.Invoke();

        int index = _rouletteViewer.GetRandomPieceIndex();
        CurrentRoulettePart = RouletteParts[index];
        PieceFound?.Invoke(CurrentRoulettePart);

        _rouletteAnimator.Spin(index, OnSpinEnd);
    }

    public void AddReward(int reward)
    {
        _rewardHolder.AddReward(reward, OnTargetRewardReached);
    }

    private void OnSpinEnd()
    {
        _isSpinning = false;
        SpinEnded?.Invoke(CurrentRoulettePart);
        _rewardHolder.ChangeView(true);
        _rewardAnimator.Spawn(this, CurrentRoulettePart.Reward, _rewardHolder.RewardIcon.sprite);
    }

    private void OnTargetRewardReached()
    {
        State = RouletteState.Cooldown;
        StartCoroutine(Cooldown(_delayBeforeCooldown));
    }

    private IEnumerator Cooldown(float delay = 0)
    {
        yield return new WaitForSeconds(delay);

        WaitForSeconds waitForSeconds = new WaitForSeconds(1);
        _currentCooldown = RouletteConfig.Cooldown;

        _rewardHolder.TryDeactivateText();

        while (_currentCooldown > 0)
        {
            CooldownUpdated?.Invoke(_currentCooldown);

            if (_currentCooldown > 0)
                _rewardHolder.GenerateNewReward();

            yield return waitForSeconds;

            _currentCooldown--;
        }

        State = RouletteState.Activate;
        RouletteActivated?.Invoke();
    }
}