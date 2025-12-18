using System;
using UnityEngine;
using UnityEngine.UI;

public record HeroData
{
    public readonly int Id;

    public readonly string Name;     //영웅 이름
    public readonly int MaxHealth;    //최대 체력
    public readonly Sprite Sprite;     //영웅 일러스트

    public HeroData(int id, StaticDataManager.HeroDataHolder dataHolder)
    {
        Id = id;
        Name = dataHolder.Name;
        MaxHealth = dataHolder.MaxHealth;
        Sprite = dataHolder.Sprite;
    }
}