using System;
using UnityEngine;
using UnityEngine.UI;

public record HeroData
{
    public readonly string Name;     //영웅 이름
    public readonly string Description;
    public readonly int MaxHealth;    //최대 체력
    public readonly Sprite Sprite;     //영웅 일러스트

    public HeroData(string name,string description, int maxHealth, Sprite sprite)
    {
        Name = name;
        Description = description;
        MaxHealth = maxHealth;
        Sprite = sprite;
    }
}