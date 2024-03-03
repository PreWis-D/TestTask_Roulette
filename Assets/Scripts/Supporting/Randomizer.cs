using System.Collections.Generic;
using UnityEngine;

public class Randomizer
{
    private int _randomTypeValue = 0;
    private RewardType _currentRewardType = RewardType.None;

    public int[] GenerateRandomNumbers(int arrayLeight, int minRandomValue, int MaxRandomValue)
    {
        int[] tempArrayLeight = new int[arrayLeight];
        List<int> tempList = new List<int>();

        for (int i = 0; i < tempArrayLeight.Length; i++)
        {
            int randomValue = Random.Range(minRandomValue, MaxRandomValue);

            if (CheckList(randomValue, tempList))
            {
                tempList.Add(randomValue);
                tempArrayLeight[i] = randomValue;
            }
            else
            {
                i--;
            }

        }

        return tempArrayLeight;
    }

    public RewardType GenerateRandomRewardType()
    {
        GetRandomType(_currentRewardType.GetHashCode());
        _currentRewardType = (RewardType)_randomTypeValue;
        return _currentRewardType;
    }

    public Vector2 GenerateRandomRadiusPosition(float minRadius, float maxRadius)
    {
        var randomXPositive = Random.Range(minRadius, maxRadius);
        var randomXNigative = Random.Range(-minRadius, -maxRadius);

        var randomYPositive = Random.Range(minRadius, maxRadius);
        var randomYNigative = Random.Range(-minRadius, -maxRadius);

        var randomX = Random.Range(0, 2);
        var randomY = Random.Range(0, 2);

        return new Vector2(
            randomX == 0 ?  randomXPositive : randomXNigative,
            randomY == 0 ? randomYPositive :  randomYNigative);
    }

    private void GetRandomType(int currentValue)
    {
        var random = Random.Range(
            RewardType.Crystals.GetHashCode()
            , RewardType.Rubies.GetHashCode() + 1);
        if (random == currentValue)
            GetRandomType(currentValue);
        else
            _randomTypeValue =  random;
    }

    private bool CheckList(int randomValue, List<int> tempList)
    {
        foreach (var value in tempList)
        {
            if (randomValue == value)
                return false;
        }

        return true;
    }
}