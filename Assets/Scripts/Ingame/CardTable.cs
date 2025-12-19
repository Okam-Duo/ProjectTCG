using UnityEngine;

[CreateAssetMenu(fileName = "CardTable", menuName = "Custom/카드 테이블", order = int.MinValue)]
public class CardTable : ScriptableObject
{
    public CardDataHolder[] cards;
}