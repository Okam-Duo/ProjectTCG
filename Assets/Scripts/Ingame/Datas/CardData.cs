using System;
using UnityEngine;
using UnityEngine.UI;

public record CardData
{
    public readonly string Name;     //카드 이름
    public readonly string Description;     //카드 설명
    public readonly CardRarity Rarity;     //카드 희귀도
    public readonly bool IsToken;    //토큰카드(덱에 편성 불가능한 카드) 여부
    public readonly Sprite Sprite;     //카드 일러스트

    public CardData(string name, string description, CardRarity rarity, bool isToken, Sprite sprite)
    {
        Name = name;
        Description = description;
        Rarity = rarity;
        IsToken = isToken;
        Sprite = sprite;
    }
}

public enum CardRarity
{
    Common,
    Rare,
    Epic,
    Legendary
}