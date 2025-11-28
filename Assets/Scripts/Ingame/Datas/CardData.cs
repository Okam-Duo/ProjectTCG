using System;
using UnityEngine;
using UnityEngine.UI;

//카드의 외형 데이터 전달용 구조체
[Serializable]
public struct CardData
{
    public enum CardRarity
    {
        #warning 카드 희귀도 목록 작성해야함
    }

    public Sprite sprite;     //카드 일러스트
    public string name;     //카드 이름
    public string description;     //카드 설명
    public CardRarity rarity;     //카드 희귀도
}