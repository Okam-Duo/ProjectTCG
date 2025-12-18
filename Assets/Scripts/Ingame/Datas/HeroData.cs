using System;
using UnityEngine;
using UnityEngine.UI;

//카드의 외형 데이터 저장용 스크립터블 오브젝트
//데이터베이스 적용 전 임시로 사용
[CreateAssetMenu(fileName = "NewHeroData", menuName = "Custom/영웅 데이터", order = int.MinValue)]
public class HeroData : ScriptableObject
{
    public int Id;

    public Sprite Sprite;     //영웅 일러스트
    public string Name;     //영웅 이름
    public int MaxHealth;    //최대 체력
}