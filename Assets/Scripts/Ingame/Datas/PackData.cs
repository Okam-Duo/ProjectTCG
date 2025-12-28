using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public record PackData
{
    public readonly string Name;     //팩 이름
    public readonly string Description;     //팩 설명
    public readonly Sprite Sprite;     //팩 일러스트
    public PackRewardData[] Rewards;     //<보상 타입, id> 배열

    public PackData(string name, string description, Sprite sprite, PackRewardData[] rewards)
    {
        Name = name;
        Description = description;
        Sprite = sprite;
        Rewards = rewards;
    }
}

[Serializable]
public struct PackRewardData
{
    public ResourceType RewardType;
    public int RewardId;
}