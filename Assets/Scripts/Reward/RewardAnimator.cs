using UnityEngine;

public class RewardAnimator : MonoBehaviour
{
    [SerializeField] private RewardIcon _rewardIconPrefab;
    [SerializeField] private Transform _rewardIconsContainer;
    [SerializeField] private RectTransform _targetTransform;
    [SerializeField] private RectTransform _rectCanvas;

    private int _maxCount = 20;
    private float _minRadiusSpawn;
    private float _maxRadiusSpawn;

    private Roulette _roulette;
    private Sprite _sprite;
    private int _reward;
    private RewardIcon[] _moneyIconPool;
    private int[] _rewardPool;

    public void Init(RouletteConfig rouletteConfig)
    {
        _minRadiusSpawn = rouletteConfig.MinRadiusSpawn;
        _maxRadiusSpawn = rouletteConfig.MaxRadiusSpawn;
        _maxCount = rouletteConfig.MaxRewardIconsCount;

        _rewardPool = new int[_maxCount];
        _moneyIconPool = new RewardIcon[_maxCount];

        for (int i = 0; i < _moneyIconPool.Length; i++)
        {
            _moneyIconPool[i] = Instantiate(_rewardIconPrefab, _rewardIconsContainer.position,
                    _rewardIconsContainer.rotation, _rewardIconsContainer.transform);
            _moneyIconPool[i].gameObject.SetActive(false);
        }
    }

    public void Spawn(Roulette roulette, int reward, Sprite sprite)
    {
        _roulette = roulette;
        _reward = reward;
        _sprite = sprite;

        ResetRewardPool();
        DistributeReward(_reward);

        for (int i = 0; i < (_reward > _maxCount ? _maxCount : _reward); i++)
        {
            if (_moneyIconPool[i].gameObject.activeSelf == false)
            {
                _moneyIconPool[i].gameObject.SetActive(true);
                _moneyIconPool[i].transform.SetParent(_rewardIconsContainer.transform);
                _moneyIconPool[i].Init(_reward <= _maxCount ? 1 : _rewardPool[i]
                    , _targetTransform, _roulette, _rectCanvas, _minRadiusSpawn, _maxRadiusSpawn, _sprite);
                _moneyIconPool[i].ShowCoin();
            }
        }
    }

    private void DistributeReward(int reward)
    {
        for (int j = 0; j < _rewardPool.Length; j++)
        {
            _rewardPool[j]++;
            reward--;

            if (reward < 1)
                return;
        }

        if (reward > 0)
            DistributeReward(reward);
    }

    private void ResetRewardPool()
    {
        for (int i = 0; i < _rewardPool.Length; i++)
        {
            _rewardPool[i] = 0;
        }
    }
}