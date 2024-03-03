using UnityEngine;

[CreateAssetMenu(fileName = "RouletteConfig", menuName = "Configs/RouletteConfig")]
public class RouletteConfig : ScriptableObject
{
    [Header("Roulette settings")]
    [SerializeField][Range(1, 20)] private int _spinDuration = 5;
    [SerializeField][Range(2, 12)] private int _partsAmount = 12;
    [SerializeField] private int _cooldown = 10;

    [Space(20)]
    [Header("Reward icons setting")]
    [SerializeField] private float _minRadiusSpawn = 20;
    [SerializeField] private float _maxRadiusSpawn = 60;
    [SerializeField] private int _maxRewardIconsCount = 20;

    [Space(20)]
    [Header("Reward range setting")]
    [SerializeField] private int _minRandomValue = 5;
    [SerializeField] private int _maxRandomValue = 101;

    public int SpinDuration => _spinDuration;
    public int PartsAmount => _partsAmount;
    public int Cooldown => _cooldown;

    public float MinRadiusSpawn => _minRadiusSpawn;
    public float MaxRadiusSpawn => _maxRadiusSpawn;
    public int MaxRewardIconsCount => _maxRewardIconsCount;

    public int MinRandomValue => _minRandomValue;
    public int MaxRandomValue => _maxRandomValue;

}