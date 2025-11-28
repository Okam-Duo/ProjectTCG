using System;
using UnityEngine;
using UnityEngine.UI;

[Serializable]
public struct CardData
{
    public enum CardRarity
    {
        #warning 카드 희귀도 목록 작성해야함
    }

    public Sprite sprite;
    public string name;
    public string description;
    public CardRarity rarity;
}