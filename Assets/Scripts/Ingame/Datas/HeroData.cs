using System;
using UnityEngine;
using UnityEngine.UI;

public record HeroData
{
    public readonly string Name;     //영웅 이름
    public readonly int MaxHealth;    //최대 체력
    public readonly Sprite Sprite;     //영웅 일러스트

    public HeroData(string name, int maxHealth, Sprite sprite)
    {
        Name = name;
        MaxHealth = maxHealth;
        Sprite = sprite;
    }
}