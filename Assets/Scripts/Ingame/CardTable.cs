using UnityEngine;

[CreateAssetMenu(fileName = "CardTable", menuName = "Custom/카드 테이블", order = int.MinValue)]
public class CardTable : ScriptableObject, IStaticDataTable<CardDataHolder>
{
    [SerializeField] private CardDataHolder[] cards;

    public CardDataHolder[] Holders => cards;
}