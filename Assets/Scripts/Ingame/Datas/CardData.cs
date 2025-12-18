using System;
using UnityEngine;
using UnityEngine.UI;

public record CardData
{
    public readonly int Id;

    public readonly string Name;     //카드 이름
    public readonly string Description;     //카드 설명
    public readonly CardRarity Rarity;     //카드 희귀도
    public readonly bool IsToken;    //토큰카드(덱에 편성 불가능한 카드) 여부
    public readonly Sprite Sprite;     //카드 일러스트

    public CardData(int id, StaticDataManager.CardDataHolder dataHolder)
    {
        Id = id;
        Name = dataHolder.Name;
        Description = dataHolder.Description;
        Rarity = dataHolder.Rarity;
        IsToken = dataHolder.IsToken;
        Sprite = dataHolder.Sprite;
    }
}

public enum CardRarity
{
#warning 카드 희귀도 목록 작성해야함
}