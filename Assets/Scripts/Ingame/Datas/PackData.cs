using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public record PackData
{
    public readonly int Id;

    public readonly string Name;     //팩 이름
    public readonly string Description;     //팩 설명
    public readonly Sprite Sprite;     //팩 일러스트
    public KeyValuePair<ResourceType,int>[] Rewards;     //<보상 타입, id> 배열

    public PackData(int id, PackDataHolder dataHolder)
    {
        Id = id;
        Name = dataHolder.Name;
        Description = dataHolder.Description;
        Sprite = dataHolder.Sprite;
        Rewards = dataHolder.Rewards;
    }
}