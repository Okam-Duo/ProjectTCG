using UnityEngine;

[CreateAssetMenu(fileName = "NewHeroData", menuName = "Custom/영웅 데이터", order = int.MinValue)]
public class HeroDataHolder : ScriptableObject
{
    [Header("영웅 일러스트")]
    public Sprite Sprite;
    [Header("영웅 이름")]
    public string Name;
    [Header("최대 체력")]
    public int MaxHealth;
}