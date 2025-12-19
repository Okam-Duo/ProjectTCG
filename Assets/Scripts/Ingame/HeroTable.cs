using UnityEngine;

[CreateAssetMenu(fileName = "HeroTable", menuName = "Custom/영웅 테이블", order = int.MinValue)]
public class HeroTable : ScriptableObject
{
    public HeroDataHolder[] heroes;
}