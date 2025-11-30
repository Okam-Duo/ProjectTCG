using System;
using UnityEngine;
using UnityEngine.UI;

//카드의 외형 데이터 저장용 스크립터블 오브젝트
//데이터베이스 적용 전 임시로 사용
[CreateAssetMenu(fileName = "NewHeroData", menuName = "Custom/영웅카드 데이터", order = int.MinValue)]
public class HeroData : ScriptableObject
{
    public Sprite Sprite;     //카드 일러스트
    public string Name;     //카드 이름
    public int MaxHealth => 10;    //최대 체력

    //스킬, 최대체력 등 데이터 추가할거면 해야됨
}