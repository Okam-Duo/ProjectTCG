using UnityEngine;

[CreateAssetMenu(fileName = "NewCardData", menuName = "Custom/카드 데이터", order = int.MinValue)]
public class CardDataHolder : ScriptableObject
{
    [Header("카드 일러스트")]
    public Sprite Sprite;
    [Header("카드 이름")]
    public string Name;
    [Header("카드 설명")]
    public string Description;
    [Header("카드 희귀도")]
    public CardRarity Rarity;
    [Header("토큰카드 여부")]
    public bool IsToken;
}