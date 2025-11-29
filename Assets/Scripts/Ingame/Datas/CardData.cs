using System;
using UnityEngine;
using UnityEngine.UI;

//카드의 외형 데이터 저장용 스크립터블 오브젝트
//데이터베이스 적용 전 임시로 사용
[CreateAssetMenu(fileName = "NewCardData", menuName = "Custom/서폿카드 데이터", order = int.MinValue)]
public class CardData : ScriptableObject
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